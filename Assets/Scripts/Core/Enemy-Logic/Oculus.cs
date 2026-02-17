using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Oculus : EnemyAbstract
    {
        //[Header("Coin")] [SerializeField] GameObject coinPrefab;
        [SerializeField] private List<GameObject> drops = new List<GameObject>();

        [SerializeField] private float maxHealth = 50f;
        [SerializeField] private float attackPower = 20f;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float attackRange = 3.5f;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float spawnFadeTime = 0.002f;

        [SerializeField] private int coinMin = 3;
        [SerializeField] private int coinMax = 7;

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