using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class SmallSlime : EnemyAbstract
    {
        [Header("Split Characteristics")]
        [SerializeField] private GameObject parentSlime;
        [SerializeField] private float statsModifier;
        [SerializeField] private float moveSpeedModifier = 4f;
        
        [Header("Drops")] 
        [SerializeField] private List<GameObject> drops = new();
       

        [Header("CoinValue")]
        [SerializeField] private int coinMin = 3;
        [SerializeField] private int coinMax = 7;

        protected override void Awake()
        {
            var slime = parentSlime.gameObject.GetComponent<EnemyAbstract>();

            MaxHealth = slime.MaxHealth * statsModifier;
            AttackPower = slime.AttackPower * statsModifier;
            MoveSpeed = slime.MoveSpeed * moveSpeedModifier;
            AttackRange = slime.AttackRange;
            Cooldown = slime.Cooldown / statsModifier;
            SpawnFadeTime = 1;
            Drops = drops;
            CoinMin = coinMin;
            CoinMax = coinMax;

            base.Awake();
        }
    }
}