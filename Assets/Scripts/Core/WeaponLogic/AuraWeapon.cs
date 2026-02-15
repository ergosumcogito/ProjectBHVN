using System;
using System.Collections.Generic;
using Core.Enemy_Logic;
using UnityEngine;

namespace Core.WeaponLogic
{


    public class AuraWeapon : WeaponBase
    {
        private float tick;
        [SerializeField] private float tickTime = 0.3f; //Time in seconds for each tick to occur

        [SerializeField] private float tickDamageReduction = 2f; // Divides final dmg because Weapon hits multiple times per second

        [SerializeField] private float meleeWeight = 1f; //The lower the number the higher the contribution

        [SerializeField] private float rangedWeight = 3f;

        // Update is called once per frame
        new void Update()
        {
            tick += Time.deltaTime;
        }

        public override float CalculateDamage()
        {

            float dmg = (meleeDamage / meleeWeight + rangeDamage / rangedWeight) / tickDamageReduction; //Default Weapon damage is 1x Melee Damage + 1/3 Ranged Damage and then gets reduced by the tickDamageReduction. Calculation divides with the reciprocal of the intended distribution to avoid using fractions in code and allow easier modification even outside of code.
            
            return CalculateCrit(dmg);
        }

        protected override void Attack(Transform target) //Can't be used on a non-aiming Weapon as it has no target
        {
            throw new NotImplementedException();
        }

        private void OnTriggerStay2D(Collider2D collide)
        {
            if (tick >= tickTime)
            {
                EnemyAbstract enemy = collide.GetComponent<EnemyAbstract>();
                enemy.TakeDamage(CalculateDamage());
                tick = 0;
            }
        }
    }
}