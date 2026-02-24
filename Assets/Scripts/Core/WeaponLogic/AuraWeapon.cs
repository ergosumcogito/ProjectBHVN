using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Enemy_Logic;

public class AuraWeapon: WeaponBase
{
    private double tick;
    [SerializeField]private double baseTickRate = 1f;
    private double finalTickRate;
    [SerializeField]private float tickDamageFactor = 2f; // 1 is full damage. The higher the lower the damage. Reason is this weapon hits every o.3 seconds. 
    [SerializeField]private float meleeWeight = 1f; // Full weight   
    [SerializeField]private float rangeWeight = 3f; //The higher the number, the lower the contribution. Base Value is 3 meaning 1/3 contribution. Avoids use of complicated decimals by using the reciprocal. Instead of multiplying by 1/3 we divide by 3.
    [SerializeField]private float radius = 1.5f; // Gets recalculated based on Transform of the weapons circle or let this decide the size of the visible circle
    
    public override void Init(WeaponData stats)
    {
        base.Init(stats);
        radius = this.GetComponentInParent<Transform>().localScale.x / 2; // x and y are the diameter so we have to divide by 2 otherwise the hitbox will be twice the size of the actual circle
        //GetComponentInParent<Transform>().localScale = new Vector3(radius*2, radius*2, 1); //If we want the circle to be based around the radius Warning: Without multiplying x and y by 2 the radius we put here will actually be the diameter and the circle could be smaller than anticipated
        if (attackSpeed > 0.5) //Prevents 1 + Math.Log(attackSpeed to go negative
        {
            finalTickRate = baseTickRate / (1 + Math.Log(attackSpeed)) < 2
                ? baseTickRate / (1 + Math.Log(attackSpeed))
                : 2;
        }
        else { finalTickRate = 2; }

}
    protected override void Attack(Transform target)
    {
        
    }

    public void Update()
    {
        tick += Time.deltaTime;

        if (tick > finalTickRate)
        {
            tick = 0;
            Debug.Log("Radius is: " + radius);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(playerStats.GetComponentInParent<Transform>().position, radius);
            foreach (var enemy in enemies)
            {
                if (!enemy.gameObject.CompareTag("Enemy")) return;
                Debug.Log("Detected enemy");
                enemy.GetComponent<EnemyAbstract>().TakeDamage(CalculateDamage());
            }
            
        }
    }

    // public void OnTriggerStay2D(Collider2D collision)
    // {
    //
    //     if (tick < finalTickRate)
    //     {
    //         Debug.Log("No damage dealt");
    //         return;
    //     }
    //     if (tick > finalTickRate)
    //     {
    //         EnemyAbstract enemy = collision.GetComponentInParent<EnemyAbstract>();
    //         if (enemy == null) return;
    //         Debug.Log("Attacking with Aura");
    //         tick = 0;
    //         enemy.TakeDamage(CalculateDamage());
    //         
    //     }
    // }

    public override float CalculateDamage()
    {
        float dmg = (meleeDamage / meleeWeight + rangeDamage / rangeWeight) / tickDamageFactor;
        return CalculateCrit(dmg);
    }
}
