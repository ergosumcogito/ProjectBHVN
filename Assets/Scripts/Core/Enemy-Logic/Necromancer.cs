using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Necromancer : EnemyAbstract
    {
        [Header("Necromancer Stats")] [SerializeField]
        private float necroMoveSpeed = 5f;

        [SerializeField] private float necroAttackPower = 25f;
        [SerializeField] private float necroMaxHealth = 70f;

        [Header("Necromancer AI Distances in tiles")] [SerializeField]
        private float fleeDistance = 10f;

        [SerializeField] private float idleMinDistance = 11f;
        [SerializeField] private float idleMaxDistance = 15f;

        public float FleeDistance => fleeDistance;
        public float IdleMinDistance => idleMinDistance;
        public float IdleMaxDistance => idleMaxDistance;

        [SerializeField] private int necroCoinMin = 10;
        [SerializeField] private int necroCoinMax = 20;
        [SerializeField] private List<GameObject> drops;

        protected override void Awake()
        {
            MoveSpeed = necroMoveSpeed;
            AttackPower = necroAttackPower;
            MaxHealth = necroMaxHealth;

            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }

        public override void Drop()
        {
            if (drops.Count <= 0) return;

            var prefab = drops[Random.Range(0, drops.Count)];

            if (!prefab.TryGetComponent<Coin>(out var component)) return;

            var coinPrefab = Instantiate(prefab, transform.position, Quaternion.identity);
            var comp = coinPrefab.GetComponent<Coin>();
            comp.CoinValue = Random.Range(necroCoinMin, necroCoinMax + 1);
        }
    }
}