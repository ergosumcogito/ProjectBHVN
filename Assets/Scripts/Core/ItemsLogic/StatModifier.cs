public enum StatType {
    MoveSpeed,
    AttackSpeed,
    MaxHealth,
    MeleeDamage,
    RangedDamage,
    CritChance,
    AttackRange
}

public enum ModifierMode {
    Add,           
    Multiply,      
    PercentAdd
}

[System.Serializable]
public struct StatModifier
{
    public StatType stat;
    public float value;
    public ModifierMode mode;
}