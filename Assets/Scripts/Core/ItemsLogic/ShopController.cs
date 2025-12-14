using UnityEngine;

public class ShopController : MonoBehaviour
{
    [Header("Hardcoded test setup")]
    [SerializeField] private ItemData testItem;
    private PlayerInventory playerInventory;

    public void BuyTestItem()
    {
        // TODO Probably risky implementation, maybe replace with player provider
        var inventory = FindAnyObjectByType<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogError("ShopController: PlayerInventory not found");
            return;
        }

        inventory.AddItem(testItem);
    }

}