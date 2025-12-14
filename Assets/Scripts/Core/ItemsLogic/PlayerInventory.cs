using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemData> items = new();

    public event Action OnInventoryChanged;

    public void AddItem(ItemData item)
    {
        items.Add(item);
        OnInventoryChanged?.Invoke();
    }
}