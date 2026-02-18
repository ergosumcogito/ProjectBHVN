using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Enemy_Logic;

public class AuraWeapon: WeaponBase
{
    private double tick;
    [SerializeField]private double baseTickRate = 0.3f;
    private double finalTickRate;
    [SerializeField] private float tickDamageFactor = 2f; // 1 is full damage. The higher the lower the damage. Reason is this weapon hits every o.3 seconds. 
    [SerializeField]private float meleeWeight = 1f; // Full weight   
    [SerializeField]private float rangeWeight = 3f; //The higher the number, the lower the contribution. Base Value is 3 meaning 1/3 contribution. Avoids use of complicated decimals by using the reciprocal. Instead of multiplying by 1/3 we divide by 3.

    public override void Init(WeaponData stats)
    {
        base.Init(stats);
        if (attackSpeed > 0.4) //Prevents 1 + Math.Log(attackSpeed to go negative
        {
            finalTickRate = baseTickRate / (1 + Math.Log(attackSpeed)) < 1
                ? baseTickRate / (1 + Math.Log(attackSpeed))
                : 1;
        }
        else { finalTickRate = 1; }
}
    protected override void Attack(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        tick += Time.deltaTime;
        if (tick > finalTickRate)
        {
            EnemyAbstract enemy = collision.GetComponentInParent<EnemyAbstract>();
            if (enemy == null) return;
        
            enemy.TakeDamage(CalculateDamage());
            tick = 0;
        }
    }

    public override float CalculateDamage()
    {
        float dmg = (meleeDamage / meleeWeight + rangeDamage / rangeWeight) / tickDamageFactor;
        return CalculateCrit(dmg);
    }
}
