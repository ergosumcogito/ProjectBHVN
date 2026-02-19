using System;
using System.Collections;
using System.Collections.Generic;
using Core.WeaponLogic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Core.Enemy_Logic
{
    public class Necromancer : EnemyAbstract
    {
        [Header("Necromancer default stats")]
        [SerializeField] private float maxHealth = 70f;
        [SerializeField] private float attackRange = -1f;
        [SerializeField] private float moveSpeed = 5f;

        [Header("Necromancer projectiles attack")]
        [SerializeField] private bool firesProjectiles = true;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float attackPower = 25f;
        [SerializeField] private float projectileSpeed = 12f;
        [SerializeField] private float cooldown = 5f;
        [SerializeField] private float firstAttackDelay = 5f;

        [Header("Necromancer default volley attack")]
        [SerializeField] private int projectileVolleyAmountMin = 3;
        [SerializeField] private int projectileVolleyAmountMax = 6;
        [SerializeField] private float projectileAngle = 6f;
        [SerializeField] private float projectileAngleMaxModifier = 2f;
        [SerializeField] private float timeBetweenProjectiles = 0.5f;
        [SerializeField] private float volleySpeedModifier = 0.8f;

        [Header("Necromancer circular projectile attack")]
        [SerializeField, Range(0f, 1f)] private float circularAttackChance = 0.2f;
        [SerializeField] private int circularProjectileAmount;
        [SerializeField] private float circularProjectileSpeedModifier = 2f;

        [Header("Necromancer summon attack")]
        [SerializeField] private GameObject summons;
        [SerializeField] private int summonAmount;
        [SerializeField] private float summonRadius;
        [SerializeField] private float cooldownSpecial = 10f;
        [SerializeField] private float firstSpecialDelay = 10f;

        [Header("Necromancer behaviour")]
        [SerializeField] private bool isFleeingType = true;
        [SerializeField] private float idleMinDistance = 20f;
        [SerializeField] private float idleMaxDistance = 30f;
        [SerializeField] private float spawnFadeTime = 2f;

        [Header("Necromancer teleport config")]
        [SerializeField] private bool canTeleportBehindPlayer = true;
        [SerializeField] private int escapeCornerMax = 10;
        [SerializeField] private int escapeCornerCounter;
        [SerializeField] private int escapeCornerSpeedMultiplier = 5;

        [Header("Necromancer coin drops")]
        [SerializeField] private List<GameObject> drops;
        [SerializeField] private int coinMin = 10;
        [SerializeField] private int coinMax = 20;

        private Coroutine _volleyRoutine;

        protected override void Awake()
        {
            MaxHealth = maxHealth;
            AttackRange = attackRange;
            MoveSpeed = moveSpeed;

            FiresProjectiles = firesProjectiles;
            Projectile = projectile;
            AttackPower = attackPower;
            ProjectileSpeed = projectileSpeed;
            Cooldown = cooldown;

            Summons = summons;
            SummonAmount = summonAmount;
            SummonRadius = summonRadius;
            CooldownSpecial = cooldownSpecial;

            IsFleeingType = isFleeingType;
            IdleMinDistance = idleMinDistance;
            IdleMaxDistance = idleMaxDistance;
            SpawnFadeTime = spawnFadeTime;

            CanTeleportBehindPlayer = canTeleportBehindPlayer;
            EscapeCornerMax = escapeCornerMax;
            EscapeCornerCounter = escapeCornerCounter;
            EscapeCornerSpeedMultiplier = escapeCornerSpeedMultiplier;

            Drops = drops;
            CoinMin = coinMin;
            CoinMax = coinMax;

            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            TimeSinceLastAttack = Time.time + firstAttackDelay;
            TimeSinceLastSpecialAttack = Time.time + firstSpecialDelay;
        }

        protected override void PerformAttack()
        {
            var now = Time.time;

            var specialReady = now - TimeSinceLastSpecialAttack >= CooldownSpecial;

            if (specialReady)
            {
                TimeSinceLastSpecialAttack = now;
                SummonAttack();
                return;
            }

            NormalRangedAttack();
        }

        private void SummonAttack()
        {
            transform.position = new Vector2(LevelBounds.x / 2f, LevelBounds.y / 2f);

            Spawner.ForceSpawnEnemy(this, summons, summonAmount, summonRadius);
        }

        private void NormalRangedAttack()
        {
            if (Random.value < circularAttackChance)
            {
                CircularAttack();
            }
            else
            {
                VolleyAttack();
            }
        }

        private void VolleyAttack()
        {
            if (_volleyRoutine != null)
            {
                StopCoroutine(_volleyRoutine);
            }

            _volleyRoutine = StartCoroutine(FireVolley());
        }

        private IEnumerator FireVolley()
        {
            var amount = Random.Range(projectileVolleyAmountMin, projectileVolleyAmountMax + 1);
            var speed = ProjectileSpeed * volleySpeedModifier;

            for (var i = 0; i < amount; i++)
            {
                var baseDir = (Player.position - transform.position).normalized;
                var modifier = Random.Range(1f, projectileAngleMaxModifier);
                var angleOffset = projectileAngle * modifier;

                angleOffset *= Random.value < 0.5f ? 1f : -1f;

                var dir = Rotate(baseDir, angleOffset);

                FireSingleProjectile(dir, speed);

                yield return new WaitForSeconds(timeBetweenProjectiles);
            }

            _volleyRoutine = null;
        }

        private void CircularAttack()
        {
            transform.position = new Vector2(LevelBounds.x / 2f, LevelBounds.y / 2f);

            var speed = projectileSpeed * circularProjectileSpeedModifier;

            var step = 360f / circularProjectileAmount;

            for (int i = 0; i < circularProjectileAmount; i++)
            {
                var angleDeg = i * step;
                var dir = AngleToDirection(angleDeg);

                FireSingleProjectile(dir, speed);
            }
        }

        private static Vector2 AngleToDirection(float angle)
        {
            var rad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }

        private void FireSingleProjectile(Vector2 dir, float speed)
        {
            var spawnPos = transform.position + (Vector3)dir * 0.5f;

            var go = Instantiate(projectile, spawnPos, Quaternion.identity);

            var proj = go.GetComponent<EnemyProjectile>();
            proj.Init(dir, speed, AttackPower);
        }

        private static Vector2 Rotate(Vector2 dir, float angle)
        {
            var rad = angle * Mathf.Deg2Rad;
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);

            return new Vector2(
                dir.x * cos - dir.y * sin,
                dir.x * sin + dir.y * cos
            );
        }
    }
}