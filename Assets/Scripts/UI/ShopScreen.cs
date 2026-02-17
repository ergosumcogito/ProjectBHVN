using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private ShopItemCard cardPrefab;

    [Header("Test Items (3 items)")]
    [SerializeField] private List<ItemData> testItems;

    private PlayerRuntimeCurrency playerCurrency;
    private PlayerRuntimeInventory playerInventory;
    
    private void Start()
    {
        playerCurrency = FindAnyObjectByType<PlayerRuntimeCurrency>();
        playerInventory = FindAnyObjectByType<PlayerRuntimeInventory>();

        if (playerCurrency == null)
            Debug.LogError("ShopScreen: PlayerRuntimeCurrency not found in scene!");
        if (playerInventory == null)
            Debug.LogError("ShopScreen: PlayerRuntimeInventory not found in scene!");
        
        BuildShop(testItems);
    }

    public void BuildShop(List<ItemData> items)
    {
        Clear();

        foreach (var item in items)
        {
            var card = Instantiate(cardPrefab, cardsContainer);
            card.Init(item);
            
            card.OnBuyClicked += (i, c) => HandleBuy(i, c);

            card.SetInteractable(playerCurrency.CanAfford(item.price));

            
            // Update button interactability dynamically when coins change
            playerCurrency.OnCoinsChanged += (coins) =>
            {
                card.SetInteractable(playerCurrency.CanAfford(item.price));
            };
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
}