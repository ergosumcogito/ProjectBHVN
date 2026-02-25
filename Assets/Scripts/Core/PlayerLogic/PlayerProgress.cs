using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProgress", menuName = "Scriptable Objects/PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    public List<ItemData> items = new();
    public List<string> weapons = new();

    public int coins;
    
    public int savedLevelIndex = 1;
    public int savedStageIndex = 1;
    
    private void OnEnable()
    {
        if (savedStageIndex < 1) savedStageIndex = 1;
        if (savedLevelIndex < 1) savedLevelIndex = 1;
    }
    
    public void ResetProgress()
    {
        items.Clear();
        weapons.Clear();
        coins = 0;
        savedStageIndex = 1;
        savedLevelIndex = 1;
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

    public void SetSavedStageAndLevel(int savedStageIndex, int savedLevelIndex)
    {
        this.savedStageIndex = savedStageIndex;
        this.savedLevelIndex = savedLevelIndex;
    }
}