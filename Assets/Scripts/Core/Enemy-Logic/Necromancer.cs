using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Enemy_Logic
{
    public class Necromancer : EnemyAbstract
    {
        [SerializeField] private float maxHealth = 70f;
        [SerializeField] private float attackPower = 25f;
        [SerializeField] private float attackRange = -1f;
        [SerializeField] private float cooldown = 5f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float idleMinDistance = 20f;
        [SerializeField] private float idleMaxDistance = 30f;

        [SerializeField] private bool canTeleportBehindPlayer = true;
        [SerializeField] private int escapeCornerMax = 10;
        [SerializeField] private int escapeCornerCounter = 0;

        [SerializeField] private float spawnFadeTime = 2f;
        [SerializeField] private bool isFleeingType = true;
        [SerializeField] private bool firesProjectiles = true;
        [SerializeField] private GameObject projectile;

        [SerializeField] private int coinMin = 10;
        [SerializeField] private int coinMax = 20;

        [SerializeField] private List<GameObject> drops;

        protected override void Awake()
        {
            MaxHealth = maxHealth;
            AttackPower = attackPower;
            MoveSpeed = moveSpeed;
            AttackRange = attackRange;
            CoolDown = cooldown;
            SpawnFadeTime = spawnFadeTime;
            IsFleeingType = isFleeingType;
            FiresProjectiles = firesProjectiles;
            Projectile = projectile;

            IdleMinDistance = idleMinDistance;
            IdleMaxDistance = idleMaxDistance;

            CanTeleportBehindPlayer = canTeleportBehindPlayer;
            EscapeCornerMax = escapeCornerMax;
            EscapeCornerCounter = escapeCornerCounter;

            CoinMin = coinMin;
            CoinMax = coinMax;

            Drops = drops;

            base.Awake();
        }
    }
}