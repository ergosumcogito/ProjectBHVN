using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData weaponData;

    [HideInInspector] public float critChance;
    [HideInInspector] public float meleeDamage;
    [HideInInspector] public float rangeDamage;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float attackSpeed;

    protected float attackCooldown;
    protected AutoAim autoAim;
    
    protected PlayerRuntimeStats playerStats;

    
    protected SpriteRenderer sr;
    private Color originalColor;
    
    
    protected virtual void Awake()
    {
        autoAim = GetComponentInParent<AutoAim>();
        if (autoAim == null)
        {
            autoAim = gameObject.AddComponent<AutoAim>();
        }
        
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
        if (autoAim == null) return;
        
        Transform target = autoAim.GetClosestEnemy();
        if (target == null) return;

        Attack(target);
        attackCooldown = 1f / attackSpeed;
    }

    protected abstract void Attack(Transform target);

    protected void ApplyStats()
    {
        meleeDamage =  playerStats.MeleeDamage * weaponData.meleeDamageScale;
        rangeDamage =  playerStats.RangedDamage * weaponData.rangedDamageScale;
        attackSpeed  = playerStats.AttackSpeed * weaponData.attackSpeedScale;
        attackRange  = playerStats.AttackRange * weaponData.attackRangeScale;
        critChance   = playerStats.CritChance * weaponData.critChanceScale;
        
        if (autoAim != null)
            autoAim.SetAttackRange(attackRange);
    }
    
    
    // Base method for damage calculation, can be overriden
    public virtual float CalculateDamage()
    {                       
        float dmg = rangeDamage > meleeDamage ? rangeDamage : meleeDamage;
        return CalculateCrit(dmg);
    }
    
    // Base method for crit calculation, can be overriden
    public virtual float CalculateCrit(float damage)
    {
        bool isCrit = Random.value < critChance;

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