using UnityEngine;

public class PlayerRuntimeStats : MonoBehaviour
{
    [SerializeField] private PlayerData baseData;

    public float MeleeDamage => baseData.meleeDamage;
    public float RangedDamage => baseData.rangedDamage;
    public float AttackSpeed => baseData.attackSpeed;
    public float AttackRange => baseData.attackRange;
    public float CritChance => baseData.critChance;
    
    public float BaseAttackRange => baseData.attackRange;

    public float FinalAttackRange { get; private set; }

    public void UpdateFinalAttackRange(float weaponAttackRange)
    {
        FinalAttackRange = weaponAttackRange;
    }
    
    private void Awake()
    {
        FinalAttackRange = BaseAttackRange;
    }
}