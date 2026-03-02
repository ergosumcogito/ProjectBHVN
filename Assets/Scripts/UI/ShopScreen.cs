using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private ItemDatabase itemDatabase;
    
    [Header("Weapons")]
    [SerializeField] private WeaponsData weaponsData;
    [SerializeField] private int weaponsInShop = 1;

    [Header("UI")]
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private ShopItemCard cardPrefab;
    [SerializeField] private CoinsHUD coinsHUD;

    [Header("Settings")]
    [SerializeField] private int itemsInShop = 3;

    private List<ShopItemCard> spawnedCards = new();

    private PlayerRuntimeCurrency playerCurrency;
    private PlayerRuntimeInventory playerInventory;

    private void OnEnable()
    {
        playerCurrency = FindAnyObjectByType<PlayerRuntimeCurrency>();
        playerInventory = FindAnyObjectByType<PlayerRuntimeInventory>();

        if (playerCurrency == null)
            Debug.LogError("ShopScreen: PlayerRuntimeCurrency not found in scene!");
        if (playerInventory == null)
            Debug.LogError("ShopScreen: PlayerRuntimeInventory not found in scene!");
        if (itemDatabase == null)
            Debug.LogError("ShopScreen: ItemDatabase is not assigned!");

        coinsHUD.Init(playerCurrency);

        playerCurrency.OnCoinsChanged += OnCoinsChanged;

        BuildRandomShop();
    }

    private void BuildRandomShop()
    {
        Clear();
        spawnedCards.Clear();

        // --- Spawn Items ---
        List<ItemData> randomItems = GetRandomItems(itemsInShop);

        foreach (var item in randomItems)
        {
            var card = Instantiate(cardPrefab, cardsContainer);
            card.Init(item);

            card.OnBuyClicked += HandleBuy;

            spawnedCards.Add(card);

            card.SetInteractable(playerCurrency.CanAfford(item.price));
        }
        
        // --- Spawn Weapon ---
        SpawnNextAvailableWeaponCard();
    }

    private List<ItemData> GetRandomItems(int count)
    {
        List<ItemData> result = new();

        if (itemDatabase == null || itemDatabase.allItems == null || itemDatabase.allItems.Count == 0)
            return result;
        
        List<ItemData> pool = new(itemDatabase.allItems);

        int itemsToTake = Mathf.Min(count, pool.Count);

        for (int i = 0; i < itemsToTake; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            result.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }

        return result;
    }

    private void OnCoinsChanged(int coins)
    {
        foreach (var card in spawnedCards)
        {
            card.RefreshInteractable(playerCurrency);
        }
    }

    private void HandleBuy(ItemData item, ShopItemCard card)
    {
        if (!playerCurrency.TrySpendCoins(item.price))
        {
            // Debug.Log("Cannot afford " + item.name);
            return;
        }

        playerInventory.AddItem(item);
        card.Hide();
    }

    private void Clear()
    {
        foreach (Transform child in cardsContainer)
            Destroy(child.gameObject);
    }

    private void OnDisable()
    {
        if (playerCurrency != null)
            playerCurrency.OnCoinsChanged -= OnCoinsChanged;
    }
    
    private void SpawnNextAvailableWeaponCard()
    {
        if (weaponsData == null || weaponsData.allWeapons.Length < 3) 
            return;

        // List of weapons
        WeaponData[] shopWeapons = new WeaponData[]
        {
            weaponsData.allWeapons[1], // Weapon 1
            weaponsData.allWeapons[2]  // Weapon 2
        };

        WeaponData weaponToSpawn = null;

        foreach (var w in shopWeapons)
        {
            if (!playerInventory.Weapons.Contains(w.weaponName))
            {
                weaponToSpawn = w;
                break; // spawn only the first not yet owned
            }
        }

        // No available weapon to spawn
        if (weaponToSpawn == null)
            return;

        // Create card
        var card = Instantiate(cardPrefab, cardsContainer);

        // Adapt WeaponData for ItemData for UI Display
        ItemData fakeItem = ScriptableObject.CreateInstance<ItemData>();
        fakeItem.itemName = weaponToSpawn.weaponName;
        fakeItem.price = weaponToSpawn.shopPrice;
        fakeItem.icon = weaponToSpawn.icon;
        fakeItem.modifiers = new List<StatModifier>();

        card.Init(fakeItem);

        card.OnBuyClicked += (item, shopCard) =>
        {
            if (!playerCurrency.TrySpendCoins(weaponToSpawn.shopPrice))
                return;

            playerInventory.AddWeapon(weaponToSpawn.weaponName);
            shopCard.Hide();
        };

        spawnedCards.Add(card);
        card.SetInteractable(playerCurrency.CanAfford(weaponToSpawn.shopPrice));
    }
    
}