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

        public float FleeDistance => fleeDistance;
        public float IdleMinDistance => idleMinDistance;
        public float IdleMaxDistance => idleMaxDistance;

        [SerializeField] private int necroCoinMin = 10;
        [SerializeField] private int necroCoinMax = 20;
        [SerializeField] private List<GameObject> drops;

        private enum RangeMode
        {
            Flee,
            Idle,
            Chase
        }

        [Header("Hysteresis (tiles)")] [SerializeField]
        private float hysteresis = 1f;

        private float FleeEnter => fleeDistance;
        private float FleeExit => fleeDistance + hysteresis;

        private float ChaseEnter => idleMaxDistance;
        private float ChaseExit => idleMaxDistance - hysteresis;

        [Header("Level Bounds")] [SerializeField]
        private GameRoundManager gameRoundManager; // assign in Inspector if possible

        private Vector2 _levelBounds; // (width, height)

        // =========================
        // Border avoidance (left-first then top/bottom)
        // =========================
        [Header("Border Avoidance")] [SerializeField]
        private float borderPadding = 5f;

        [SerializeField] private float offsetFromBorder = 1f;

        private enum FirstBorder
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        private FirstBorder _firstBorder = FirstBorder.None;

        // =========================
        // Forced movement (MoveTo)
        // =========================
        [Header("Forced Movement")] [SerializeField]
        private float arriveDistance = 0.2f;

        private bool _isForcedMoving;
        private Vector2 _forcedTarget;

        private RangeMode _currentMode;

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

            // If not assigned in Inspector, try to find it (safe for uni projects)
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

            if (position.x <= borderPadding || position.x >= _levelBounds.x - borderPadding)
            {
                FindClosestBorder();
            }
            else if (position.y <= borderPadding || position.y >= _levelBounds.y - borderPadding)
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

            if (position.x <= borderPadding)
            {
                _firstBorder = FirstBorder.Left;
            }
            else if (position.x >= _levelBounds.x - borderPadding)
            {
                _firstBorder = FirstBorder.Right;
            }
            else if (position.y >= _levelBounds.y - borderPadding)
            {
                _firstBorder = FirstBorder.Top;
            }
            else if (position.y <= borderPadding)
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
                    if (position.y <= borderPadding)
                    {
                        Debug.Log("left + bottom");
                        MoveTo(new Vector2(xPrimOffset[0], ySndOffset[0]));
                    }

                    if (position.y >= _levelBounds.y - borderPadding)
                    {
                        Debug.Log("left + top");
                        MoveTo(new Vector2(xPrimOffset[0], ySndOffset[1]));
                    }

                    break;
                case FirstBorder.Right:
                    if (position.y <= borderPadding)
                    {
                        Debug.Log("right + bottom");
                        MoveTo(new Vector2(xPrimOffset[1], ySndOffset[0]));
                    }

                    if (position.y >= _levelBounds.y - borderPadding)
                    {
                        Debug.Log("right + top");
                        MoveTo(new Vector2(xPrimOffset[1], ySndOffset[1]));
                    }

                    break;
                case FirstBorder.Top:
                    if (position.x <= borderPadding)
                    {
                        Debug.Log("top + left");
                        MoveTo(new Vector2(xSndOffset[0], yPrimOffset[1]));
                    }

                    if (position.x >= _levelBounds.x - borderPadding)
                    {
                        Debug.Log("top + right");
                        MoveTo(new Vector2(xSndOffset[1], yPrimOffset[1]));
                    }

                    break;
                case FirstBorder.Bottom:
                    if (position.x <= borderPadding)
                    {
                        Debug.Log("bottom + left");
                        MoveTo(new Vector2(xSndOffset[0], yPrimOffset[0]));
                    }

                    if (position.x >= _levelBounds.x - borderPadding)
                    {
                        Debug.Log("bottom + right");
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
                    MovementDirection =
                        ((Vector2)transform.position - (Vector2)Player.position).normalized;
                    SetAnimationState(chasing: true, attacking: false, dead: false);
                    break;

                case RangeMode.Chase:
                    MovementDirection =
                        ((Vector2)Player.position - (Vector2)transform.position).normalized;
                    SetAnimationState(chasing: true, attacking: false, dead: false);
                    break;

                case RangeMode.Idle:
                    MovementDirection = Vector2.zero;
                    SetAnimationState(chasing: false, attacking: false, dead: false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        private void MoveTo(Vector2 target)
        {
            if (_isForcedMoving && Vector2.Distance(_forcedTarget, target) < 0.25f)
                return;

            _forcedTarget = target;
            _isForcedMoving = true;
        }

        private void HandleForcedMove()
        {
            float d = Vector2.Distance(transform.position, _forcedTarget);

            if (d <= arriveDistance)
            {
                _isForcedMoving = false;
                MovementDirection = Vector2.zero;
                SetAnimationState(false, false, false);
                return;
            }

            Vector2 dir = (_forcedTarget - (Vector2)transform.position).normalized;
            MovementDirection = dir;

            // Uses the same move anim for forced movement
            SetAnimationState(true, false, false);
        }
    }
}