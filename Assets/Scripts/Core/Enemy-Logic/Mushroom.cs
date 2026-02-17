using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Mushroom : EnemyAbstract
    {
        [SerializeField] private List<GameObject> drops;
        [SerializeField] private float maxHealth = 70f;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float attackRange = -1f;
        [SerializeField] private float spawnFadeTime = 2f;

        [SerializeField] private int coinMin = 10;
        [SerializeField] private int coinMax = 20;

        [Header("Mushroom behaviour")] 
        [SerializeField] private bool isFleeingType = true;
        [SerializeField] private float idleMinDistance = 20f;
        [SerializeField] private float idleMaxDistance = 30f;
        [SerializeField] private int escapeCornerSpeedMultiplier = 5;
        [SerializeField] private bool canTeleportBehindPlayer;
        
        [Header("Necromancer projectiles attack")]
        [SerializeField] private bool firesProjectiles = true;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float attackPower = 25f;
        [SerializeField] private float projectileSpeed = 12f;
        [SerializeField] private float cooldown = 5f;
        [SerializeField] private float firstAttackDelay = 5f;
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
            
            IsFleeingType = isFleeingType;
            IdleMinDistance = idleMinDistance;
            IdleMaxDistance = idleMaxDistance;
            SpawnFadeTime = spawnFadeTime;
            EscapeCornerSpeedMultiplier = escapeCornerSpeedMultiplier;
            CanTeleportBehindPlayer = canTeleportBehindPlayer;
            Drops = drops;
            CoinMin = coinMin;
            CoinMax = coinMax;
            

            base.Awake();
        }
    }
}