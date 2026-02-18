using UnityEngine;

public class ItemsHUD : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private ItemIconView iconPrefab;

    private PlayerRuntimeInventory inventory;

    public void Init(PlayerRuntimeInventory inventory)
    {
        if (this.inventory != null)
            this.inventory.OnInventoryChanged -= Refresh;

        this.inventory = inventory;

        inventory.OnInventoryChanged += Refresh;

        Refresh();
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;
    }

    private void Refresh()
    {
        // clean
        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);

        // create icons
        foreach (var item in inventory.Items)
        {
            var icon = Instantiate(iconPrefab, container);
            icon.Setup(item.icon);
        }
    }
}