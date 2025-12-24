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
        }
    }

    private void Clear()
    {
        foreach (Transform child in cardsContainer)
            Destroy(child.gameObject);
    }
}