using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private ItemDatabase itemDatabase;

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

        List<ItemData> randomItems = GetRandomItems(itemsInShop);

        foreach (var item in randomItems)
        {
            var card = Instantiate(cardPrefab, cardsContainer);
            card.Init(item);

            card.OnBuyClicked += HandleBuy;

            spawnedCards.Add(card);

            card.SetInteractable(playerCurrency.CanAfford(item.price));
        }
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
}