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

        protected override void Update()
        {
            // Run state machine etc.
            stateManager?.Update();

            if (IsDead || Player == null) return;

            FacePlayer();
        }

        [SerializeField] private float faceDeadZone = 0.05f; // tweak: 0.02–0.1
        [SerializeField] private float minFlipInterval = 0.1f; // tweak: 0–0.2
        private float lastFlipTime;

        private void FacePlayer()
        {
            float dx = Player.position.x - transform.position.x;

            if (Mathf.Abs(dx) < faceDeadZone)
                return;

            if (Time.time - lastFlipTime < minFlipInterval)
                return;

            bool shouldFaceRight = dx > 0f;

            if (shouldFaceRight != facingRight)
            {
                Flip();
                lastFlipTime = Time.time;
            }
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