using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Slime : EnemyAbstract
    {
        //[Header("Coin")] [SerializeField] GameObject coinPrefab;


        [Header("Slime splits when attacked")]
        [SerializeField] private GameObject enemyPrefab;

        [SerializeField] private float splitRadius;
        [SerializeField] private int splitAmount = 3;

        [SerializeField] private float maxHealth = 50f;
        [SerializeField] private float attackPower = 20f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackRange = 3.5f;
        [SerializeField] private float cooldown = 2f;
        [SerializeField] private float spawnFadeTime = 0.002f;
        [SerializeField] private bool isSplitting = true;

        protected override void Awake()
        {
            MaxHealth = maxHealth;
            AttackPower = attackPower;
            MoveSpeed = moveSpeed;
            AttackRange = attackRange;
            Cooldown = cooldown;
            SpawnFadeTime = spawnFadeTime;
            Summons = enemyPrefab;
            SummonAmount = splitAmount;
            SummonRadius = splitRadius;
            IsSplitting = isSplitting;
            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }

        private void Split()
        {
            transform.position = new Vector2(LevelBounds.x / 2f, LevelBounds.y / 2f);

            for (var i = 0; i < SummonAmount; i++)
            {
                var offset = Random.insideUnitCircle * SummonRadius;
                var spawnPos = transform.position + (Vector3)offset;

                Instantiate(Summons, spawnPos, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}