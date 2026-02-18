using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class SmallSlime : EnemyAbstract
    {
        [SerializeField] private GameObject parentSlime;    
        [SerializeField] private int statsModifier;    
        [SerializeField] private List<GameObject> drops = new();
        
        [SerializeField] private int coinMin = 3;
        [SerializeField] private int coinMax = 7;
        protected override void Awake()
        {
            var slime = parentSlime.gameObject.GetComponent<EnemyAbstract>();

            MaxHealth = slime.MaxHealth * statsModifier;
            AttackPower = slime.AttackPower * statsModifier;
            MoveSpeed = slime.MoveSpeed / statsModifier;
            AttackRange = slime.AttackRange;
            Cooldown = slime.Cooldown / statsModifier;
            SpawnFadeTime = 1;

            CoinMin = coinMin;
            CoinMax = coinMax;

            base.Awake();
        }
    }
}