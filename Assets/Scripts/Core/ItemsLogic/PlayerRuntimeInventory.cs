using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuntimeInventory : MonoBehaviour
{
    private PlayerProgress progress;

    public IReadOnlyList<ItemData> Items => progress.items;

    public event Action OnInventoryChanged;

    public void Init(PlayerProgress progress)
    {
        this.progress = progress;
    }

    public void AddItem(ItemData item)
    {
        progress.AddItem(item);
        OnInventoryChanged?.Invoke();
    }

    public void AddWeapon(string weaponName)
    {
        progress.AddWeapon(weaponName);
        OnInventoryChanged?.Invoke();
    }
}