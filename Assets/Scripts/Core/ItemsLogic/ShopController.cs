using UnityEngine;

public class ShopController : MonoBehaviour
{
    [Header("Hardcoded test setup")]
    [SerializeField] private ItemData testItem;

    public void BuyTestItem()
    {
        // TODO Probably risky implementation, maybe replace with player provider
        var inventory = FindAnyObjectByType<PlayerRuntimeInventory>();

        if (inventory == null)
        {
            Debug.LogError("ShopController: PlayerInventory not found");
            return;
        }

        inventory.AddItem(testItem);
    }

}