using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private ShopItemCard cardPrefab;
    [SerializeField] private CoinsHUD coinsHUD;

    [Header("Test Items (3 items)")]
    [SerializeField] private List<ItemData> testItems;

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
        
        coinsHUD.Init(playerCurrency);
        
        playerCurrency.OnCoinsChanged += OnCoinsChanged;

        
        BuildShop(testItems);
    }

    public void BuildShop(List<ItemData> items)
    {
        Clear();
        spawnedCards.Clear();

        foreach (var item in items)
        {
            var card = Instantiate(cardPrefab, cardsContainer);
            card.Init(item);
            
            card.OnBuyClicked += HandleBuy;
            
            spawnedCards.Add(card);
            
            card.SetInteractable(playerCurrency.CanAfford(item.price));
        }
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