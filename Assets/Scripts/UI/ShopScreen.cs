using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private ShopItemCard cardPrefab;

    [Header("Test Items (3 items)")]
    [SerializeField] private List<ItemData> testItems;

    private void Start()
    {
        BuildShop(testItems);
    }

    public void BuildShop(List<ItemData> items)
    {
        Clear();

        foreach (var item in items)
        {
            var card = Instantiate(cardPrefab, cardsContainer);
            card.Init(item);
            card.OnBuyClicked += HandleBuy;
        }
    }
    
    private void HandleBuy(ItemData item, ShopItemCard card)
    {
        // later we can upgrade:
        // if (!CanAfford(item)) return;

        var inventory = FindAnyObjectByType<PlayerRuntimeInventory>();

        if (inventory == null)
        {
            Debug.LogError("ShopController: PlayerInventory not found");
            return;
        }

        inventory.AddItem(item);
        card.Hide();
    }

    private void Clear()
    {
        foreach (Transform child in cardsContainer)
            Destroy(child.gameObject);
    }
}