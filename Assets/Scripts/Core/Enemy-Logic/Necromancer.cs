using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public override float FleeDistance => fleeDistance;
        public override float IdleMinDistance => idleMinDistance;
        public override float IdleMaxDistance => idleMaxDistance;
        public override Vector2 LevelBounds => _levelBounds;

        [SerializeField] private int necroCoinMin = 10;
        [SerializeField] private int necroCoinMax = 20;
        [SerializeField] private List<GameObject> drops;

        private int _escapedCorner;
        private const int EscapedMax = 10;
        private const int TeleportOffset = 4;

        private enum RangeMode
        {
            Flee,
            Idle,
            Chase
        }

        private RangeMode _currentMode;

        [Header("Hysteresis (tiles)")] [SerializeField]
        private float hysteresis = 1f;

        private float FleeEnter => fleeDistance;
        private float FleeExit => fleeDistance + hysteresis;

        private float ChaseEnter => idleMaxDistance;
        private float ChaseExit => idleMaxDistance - hysteresis;

        [Header("Level Bounds")] [SerializeField]
        private GameRoundManager gameRoundManager;

        private Vector2 _levelBounds;

        private const float BorderPadding = 5f;

        private enum FirstBorder
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        private FirstBorder _firstBorder = FirstBorder.None;

        [Header("Forced Movement")] [SerializeField]
        private float arriveDistance = 0.2f;

        private bool _isForcedMoving;
        private Vector2 _forcedTarget;

        protected override void Awake()
        {
            MoveSpeed = necroMoveSpeed;
            AttackPower = necroAttackPower;
            MaxHealth = necroMaxHealth;

            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            if (gameRoundManager == null)
                gameRoundManager = FindFirstObjectByType<GameRoundManager>();

            if (gameRoundManager != null)
            {
                _levelBounds = gameRoundManager.GetCurrentLevelBounds();
            }
            else
            {
                Debug.LogWarning($"{name}: GameRoundManager not found. Border avoidance will use (0,0) bounds.");
                _levelBounds = Vector2.zero;
            }
        }

        protected override void Update()
        {
            stateManager?.Update();

            if (IsDead || !Player) return;

            if (_isForcedMoving)
            {
                HandleForcedMove();
                FacePlayer();
                return;
            }

            CheckDistanceToBorder();

            FacePlayer();
            HandleDistance();
        }

        private void FacePlayer()
        {
            var diff = Player.position.x - transform.position.x;

            var shouldFaceRight = diff > 0f;

            if (shouldFaceRight != facingRight) Flip();
        }

        private void CheckDistanceToBorder()
        {
            Vector2 position = transform.position;

            if (position.x <= BorderPadding || position.x >= _levelBounds.x - BorderPadding ||
                position.y <= BorderPadding || position.y >= _levelBounds.y - BorderPadding)
            {
                FindClosestBorder();
            }
            else
            {
                _firstBorder = FirstBorder.None;
                Debug.Log(_firstBorder);
            }
        }

        private void FindClosestBorder()
        {
            if (_firstBorder != FirstBorder.None)
            {
                HandleFirstBorder();
                return;
            }

            Vector2 position = transform.position;

            if (position.x <= BorderPadding)
            {
                _firstBorder = FirstBorder.Left;
            }
            else if (position.x >= _levelBounds.x - BorderPadding)
            {
                _firstBorder = FirstBorder.Right;
            }
            else if (position.y >= _levelBounds.y - BorderPadding)
            {
                _firstBorder = FirstBorder.Top;
            }
            else if (position.y <= BorderPadding)
            {
                _firstBorder = FirstBorder.Bottom;
            }
        }

        private void HandleFirstBorder()
        {
            Vector2 position = transform.position;

            float[] xPrimOffset = { 12f, _levelBounds.x - 12f };
            float[] xSndOffset = { 6f, _levelBounds.x - 6f };

            float[] yPrimOffset = { 12f, _levelBounds.y - 12f };
            float[] ySndOffset = { 6f, _levelBounds.y - 6f };

            switch (_firstBorder)
            {
                case FirstBorder.Left:
                    if (position.y <= BorderPadding)
                    {
                        if (TeleportBehindPlayer("top bottom")) return;
                        MoveTo(new Vector2(xPrimOffset[0], ySndOffset[0]));
                    }

                    if (position.y >= _levelBounds.y - BorderPadding)
                    {
                        if (TeleportBehindPlayer("top left")) return;
                        MoveTo(new Vector2(xPrimOffset[0], ySndOffset[1]));
                    }

                    break;
                case FirstBorder.Right:
                    if (position.y <= BorderPadding)
                    {
                        if (TeleportBehindPlayer("bottom right")) return;
                        MoveTo(new Vector2(xPrimOffset[1], ySndOffset[0]));
                    }

                    if (position.y >= _levelBounds.y - BorderPadding)
                    {
                        if (TeleportBehindPlayer("top right")) return;
                        MoveTo(new Vector2(xPrimOffset[1], ySndOffset[1]));
                    }

                    break;
                case FirstBorder.Top:
                    if (position.x <= BorderPadding)
                    {
                        if (TeleportBehindPlayer("top left")) return;
                        MoveTo(new Vector2(xSndOffset[0], yPrimOffset[1]));
                    }

                    if (position.x >= _levelBounds.x - BorderPadding)
                    {
                        if (TeleportBehindPlayer("top right")) return;
                        MoveTo(new Vector2(xSndOffset[1], yPrimOffset[1]));
                    }

                    break;
                case FirstBorder.Bottom:
                    if (position.x <= BorderPadding)
                    {
                        if (TeleportBehindPlayer("bottom left")) return;
                        MoveTo(new Vector2(xSndOffset[0], yPrimOffset[0]));
                    }

                    if (position.x >= _levelBounds.x - BorderPadding)
                    {
                        if (TeleportBehindPlayer("bottom right")) return;
                        MoveTo(new Vector2(xSndOffset[1], yPrimOffset[0]));
                    }

                    break;
            }
        }

        private void HandleDistance()
        {
            var distance = Vector2.Distance(transform.position, Player.position);

            switch (_currentMode)
            {
                case RangeMode.Flee:
                    if (distance >= FleeExit)
                        _currentMode = RangeMode.Idle;
                    break;

                case RangeMode.Idle:
                    if (distance <= FleeEnter)
                        _currentMode = RangeMode.Flee;
                    else if (distance >= ChaseEnter)
                        _currentMode = RangeMode.Chase;
                    break;

                case RangeMode.Chase:
                    if (distance <= ChaseExit)
                        _currentMode = RangeMode.Idle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetAnimation();
        }

        private void SetAnimation()
        {
            switch (_currentMode)
            {
                case RangeMode.Flee:
                case RangeMode.Chase:
                    SetAnimationState(chasing: true, attacking: false, dead: false);
                    break;

                case RangeMode.Idle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Drop()
        {
            if (drops.Count <= 0) return;

            var prefab = drops[Random.Range(0, drops.Count)];

            if (!prefab.TryGetComponent<Coin>(out _)) return;

            var coinPrefab = Instantiate(prefab, transform.position, Quaternion.identity);
            var comp = coinPrefab.GetComponent<Coin>();
            comp.CoinValue = Random.Range(necroCoinMin, necroCoinMax + 1);
        }

        private void MoveTo(Vector2 target)
        {
            if (_isForcedMoving && Vector2.Distance(_forcedTarget, target) < 0.25f)
                return;

            _forcedTarget = target;
            _isForcedMoving = true;

            _escapedCorner++;
        }

        private void HandleForcedMove()
        {
            var d = Vector2.Distance(transform.position, _forcedTarget);

            if (d <= arriveDistance)
            {
                _isForcedMoving = false;
                MovementDirection = Vector2.zero;
                SetAnimationState(false, false, false);
                return;
            }

            var dir = (_forcedTarget - (Vector2)transform.position).normalized;
            MovementDirection = dir;

            SetAnimationState(true, false, false);
        }

        private bool TeleportBehindPlayer(string corner)
        {
            var chance = (float)_escapedCorner / EscapedMax;

            var roll = Random.value;

            if (roll <= chance)
            {
                switch (corner)
                {
                    case "bottom left":
                        transform.position = Player.position + new Vector3(TeleportOffset, TeleportOffset);
                        break;
                    case "bottom right":
                        transform.position = Player.position + new Vector3(-TeleportOffset, TeleportOffset);
                        break;
                    case "top left":
                        transform.position = Player.position + new Vector3(TeleportOffset, -TeleportOffset);
                        break;
                    case "top right":
                        transform.position = Player.position + new Vector3(-TeleportOffset, -TeleportOffset);
                        break;
                }
                
                _escapedCorner = 0;
                return true;
            }

            return false;
        }
    }
}