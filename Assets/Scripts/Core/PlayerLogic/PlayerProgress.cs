using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProgress", menuName = "Scriptable Objects/PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    public List<ItemData> items = new();
    public List<string> weapons = new();

    public int coins;
    
    public void ResetProgress()
    {
        items.Clear();
        weapons.Clear();
        coins = 0;
    }

    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    public void AddWeapon(string weaponName)
    {
        if (!weapons.Contains(weaponName))
            weapons.Add(weaponName);
    }
}