using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Skeleton : EnemyAbstract
    {
        [Header("Drops")] 
        [SerializeField] private List<GameObject> drops= new ();

        [Header("BaseStats")] 
        [SerializeField] private float maxHealth = 90f;
        [SerializeField] private float attackPower = 50f;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float attackRange = 3.5f;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float spawnFadeTime = 0.002f;

        [Header("CoinValue")]
        [SerializeField] private int coinMin = 20;
        [SerializeField] private int coinMax = 50;

        protected override void Awake()
        {
            MaxHealth = maxHealth;
            AttackPower = attackPower;
            MoveSpeed = moveSpeed;
            AttackRange = attackRange;
            Cooldown = cooldown;
            SpawnFadeTime = spawnFadeTime;
            Drops = drops;

            CoinMin = coinMin;
            CoinMax = coinMax;

            base.Awake();
        }
    }
}