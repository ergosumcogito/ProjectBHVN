using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Skeleton : EnemyAbstract
    {
        [SerializeField] private List<GameObject> drops;

        [SerializeField] private float maxHealth = 90f;
        [SerializeField] private float attackPower = 50f;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float attackRange = 3.5f;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float spawnFadeTime = 0.002f;

        [SerializeField] private int coinMin = 20;
        [SerializeField] private int coinMax = 50;

        protected override void Awake()
        {
            MaxHealth = maxHealth;
            AttackPower = attackPower;
            MoveSpeed = moveSpeed;
            AttackRange = attackRange;
            CoolDown = cooldown;
            SpawnFadeTime = spawnFadeTime;
            Drops = drops;

            CoinMin = coinMin;
            CoinMax = coinMax;

            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }
    }
}