using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData weaponData;

    [HideInInspector] public float baseCritChance;
    [HideInInspector] public float baseMeleeDamage;
    [HideInInspector] public float baseRangeDamage;
    [HideInInspector] public float baseAttackRange;
    [HideInInspector] public float baseAttackSpeed;

    protected float attackCooldown;
    protected AutoAim autoAim;
    
    protected PlayerRuntimeStats playerStats;

    
    protected SpriteRenderer sr;
    private Color originalColor;
    
    
    protected virtual void Awake()
    {
        autoAim = GetComponentInParent<AutoAim>();
        playerStats = GetComponentInParent<PlayerRuntimeStats>();

        
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
            originalColor = sr.color;
    }

    public virtual void Init(WeaponData data)
    {
        weaponData = data;
        ApplyStats();
    }

    protected virtual void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f)
            TryAttack();
    }

    protected virtual void TryAttack()
    {
        Transform target = autoAim.GetClosestEnemy();
        if (target == null) return;

        Attack(target);
        attackCooldown = 1f / baseAttackSpeed;
    }

    protected abstract void Attack(Transform target);

    protected void ApplyStats()
    {
        baseMeleeDamage =  playerStats.MeleeDamage * weaponData.meleeDamageScale;
        baseRangeDamage =  playerStats.RangedDamage * weaponData.rangedDamageScale;
        baseAttackSpeed  = playerStats.AttackSpeed * weaponData.attackSpeedScale;
        baseAttackRange  = playerStats.AttackRange * weaponData.attackRangeScale;
        baseCritChance   = playerStats.CritChance * weaponData.critChanceScale;
        
        // TODO apply weapon attack range modifiers also
       // playerStats.UpdateFinalAttackRange(baseAttackRange);
    }
    
    
    // Base method for damage calculation, can be overriden
    public virtual float CalculateDamage()
    {                       
        float dmg = baseRangeDamage > baseMeleeDamage ? baseRangeDamage : baseMeleeDamage;
        return CalculateCrit(dmg);
    }
    
    // Base method for crit calculation, can be overriden
    public virtual float CalculateCrit(float damage)
    {
        bool isCrit = Random.value < baseCritChance;

        // x2 damage when crit, this is our base logic (Brotato also has x2 as base)
        
        if (isCrit)
        {
            FlashCritColor();
            return damage * 2f; 
        }

        return damage;
    }
    
    private void FlashCritColor()
    {
        if (sr != null)
        {
            StopAllCoroutines();
            StartCoroutine(CritFlashCoroutine());
        }
    }
    
    private IEnumerator CritFlashCoroutine()
    {
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.45f); // crit animation duration
        sr.color = originalColor;
    }
    
}