using System.Collections.Generic;
using UnityEngine;

public class PlayerRuntimeStats : MonoBehaviour
{
    [SerializeField] private PlayerData baseData;
    [SerializeField] private PlayerInventory inventory;

    private Dictionary<StatType, float> finalStats = new();
    
    public event System.Action OnStatsChanged;

    public float MeleeDamage => GetStat(StatType.MeleeDamage);
    public float RangedDamage => GetStat(StatType.RangedDamage);
    public float AttackSpeed => GetStat(StatType.AttackSpeed);
    public float AttackRange => GetStat(StatType.AttackRange);
    public float CritChance => GetStat(StatType.CritChance);
    public float MoveSpeed => GetStat(StatType.MoveSpeed);
    public float MaxHealth => GetStat(StatType.MaxHealth);

    private void Awake()
    {
        RecalculateStats();
    }

    public void RecalculateStats()
    {
        finalStats.Clear();

        // 1. Get base values
        finalStats[StatType.MeleeDamage] = baseData.meleeDamage;
        finalStats[StatType.RangedDamage] = baseData.rangedDamage;
        finalStats[StatType.AttackSpeed]  = baseData.attackSpeed;
        finalStats[StatType.AttackRange]  = baseData.attackRange;
        finalStats[StatType.CritChance]   = baseData.critChance;
        finalStats[StatType.MoveSpeed]    = baseData.moveSpeed;
        finalStats[StatType.MaxHealth]    = baseData.maxHealth;

        // 2. Apply items
        foreach (var item in inventory.items)
        {
            ApplyItem(item);
        }
        
        OnStatsChanged?.Invoke();
    }

    private void ApplyItem(ItemData item)
    {
        foreach (var mod in item.modifiers)
        {
            ApplyModifier(mod);
        }
    }

    private void ApplyModifier(StatModifier mod)
    {
        float current = finalStats[mod.stat];

        switch (mod.mode)
        {
            case ModifierMode.Add:
                current += mod.value;
                break;

            case ModifierMode.Multiply:
                current *= mod.value;
                break;

            case ModifierMode.PercentAdd:
                current *= (1f + mod.value / 100f);
                break;
        }

        finalStats[mod.stat] = current;
    }

    private float GetStat(StatType stat)
    {
        return finalStats.TryGetValue(stat, out var value) ? value : 0f;
    }
    
    private void OnEnable()
    {
        inventory.OnInventoryChanged += RecalculateStats;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= RecalculateStats;
    }

}
