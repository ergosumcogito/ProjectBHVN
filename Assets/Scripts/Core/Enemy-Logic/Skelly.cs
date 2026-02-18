using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Skelly : EnemyAbstract
    {
        [SerializeField] private float maxHealth = 90f;
        [SerializeField] private float attackPower = 50f;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float attackRange = 3.5f;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float spawnFadeTime = 1f;

        [SerializeField] private int coinMin = 1;
        [SerializeField] private int coinMax = 1;

        [SerializeField] private List<GameObject> drops;
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