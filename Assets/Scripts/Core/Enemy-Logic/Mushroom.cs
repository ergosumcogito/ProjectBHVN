using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Mushroom : EnemyAbstract
    {
        [SerializeField] private List<GameObject> drops;

        [SerializeField] private float maxHealth = 70f;
        [SerializeField] private float attackPower = 25f;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float attackRange = 3.5f;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float spawnFadeTime = 0.002f;

        [SerializeField] private int coinMin = 10;
        [SerializeField] private int coinMax = 20;

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

            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }
    }
}