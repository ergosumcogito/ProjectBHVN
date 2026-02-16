using System.Linq;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyFleeState : EnemyBaseState
    {
        private enum Border
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        private Border _firstBorder = Border.None;
        private Border _secondBorder = Border.None;
        private const int BorderPadding = 4;
        private const int TilesToRunAway = 12;
        private const int _moveSpeedMultiplier = 3;
        private bool _isEscapingCorner;

        private float _normalMoveSpeed;

        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            // Debug.Log("Entered Flee State");

            enemy.canMove = true;

            enemy.SetAnimationState(
                new AnimationStateChange(AnimationBool.IsChasing, true),
                new AnimationStateChange(AnimationBool.IsAttacking, false),
                new AnimationStateChange(AnimationBool.IsInactive, false),
                new AnimationStateChange(AnimationBool.IsDead, false),
                new AnimationStateChange(AnimationBool.IsIdle, false));
            enemy.UnfreezeEnemy();
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }

            if (enemy.IsFleeingType)
            {
                if (Time.time - enemy.TimeSinceLastAttack > enemy.CoolDown)
                {
                    manager.SwitchState(manager.EnemyAttackState);
                    return;
                }
            }

            var distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);

            if (distance >= enemy.IdleMinDistance)
            {
                manager.SwitchState(manager.EnemyIdleState);
                return;
            }

            FleeFromPlayer(enemy);
        }

        private void FleeFromPlayer(EnemyAbstract enemy)
        {
            Vector2 enemyPos = enemy.transform.position;
            Vector2 playerPos = enemy.Player.position;
            var bounds = enemy.LevelBounds;

            if (_isEscapingCorner)
            {
                if (!HandleCorner(enemy, enemyPos, bounds))
                {
                    enemy.MoveSpeed = _normalMoveSpeed;
                    enemy.EscapeCornerCounter++;
                    _secondBorder = Border.None;
                    SetFirstBorder(enemyPos, bounds);
                }

                return;
            }

            var (close, far) = DistanceToBorder(enemyPos, bounds);
            float[] values = { close.x, close.y, far.x, far.y };
            var nearAnyBorder = values.Any(val => val is >= 0f and <= BorderPadding);

            // Debug.Log($"close borders: {close}");
            // Debug.Log($"far borders: {far}");
            //
            // Debug.Log($"near any border? {nearAnyBorder}");
            //
            // Debug.Log($"first border: {_firstBorder}");
            // Debug.Log($"second border: {_secondBorder}");

            if (!nearAnyBorder)
            {
                _firstBorder = Border.None;
                _secondBorder = Border.None;
            }
            else
            {
                if (_firstBorder == Border.None) SetFirstBorder(enemyPos, bounds);

                if (_firstBorder != Border.None) SetSecondBorder(enemyPos, bounds);
            }

            if (_firstBorder != Border.None && _secondBorder == Border.None)
            {
                HandleFirstBorder(enemy, enemyPos, playerPos);
                return;
            }

            if (_firstBorder != Border.None && _secondBorder != Border.None)
            {
                if (!_isEscapingCorner && TryTeleport(enemy)) return;
                HandleCorner(enemy, enemyPos, bounds);

                _normalMoveSpeed = enemy.MoveSpeed;
                enemy.MoveSpeed = _normalMoveSpeed * _moveSpeedMultiplier;

                return;
            }

            var direction = (enemyPos - playerPos).normalized;
            enemy.movementDirection = direction;
        }

        public void SetFirstBorder(Vector2 position, Vector2 bounds)
        {
            _firstBorder = Border.None;

            if (TooCloseToLeft(position))
                _firstBorder = Border.Left;
            else if (TooCloseToRight(position, bounds))
                _firstBorder = Border.Right;
            else if (TooCloseToTop(position, bounds))
                _firstBorder = Border.Top;
            else if (TooCloseToBottom(position))
                _firstBorder = Border.Bottom;
        }

        public void SetSecondBorder(Vector2 position, Vector2 bounds)
        {
            _secondBorder = Border.None;

            if (_firstBorder != Border.Left && TooCloseToLeft(position))
                _secondBorder = Border.Left;
            else if (_firstBorder != Border.Right && TooCloseToRight(position, bounds))
                _secondBorder = Border.Right;
            else if (_firstBorder != Border.Top && TooCloseToTop(position, bounds))
                _secondBorder = Border.Top;
            else if (_firstBorder != Border.Bottom && TooCloseToBottom(position))
                _secondBorder = Border.Bottom;
        }

        private void HandleFirstBorder(EnemyAbstract enemy, Vector2 enemyPos, Vector2 playerPos)
        {
            // Debug.Log($"enemy position: {enemyPos}");
            // Debug.Log($"player position: {playerPos}");

            if (_firstBorder is Border.Left or Border.Right)
            {
                if (enemyPos.y < playerPos.y)
                {
                    enemy.movementDirection = Vector2.down;
                    return;
                }

                if (enemyPos.y >= playerPos.y)
                {
                    enemy.movementDirection = Vector2.up;
                    return;
                }
            }

            if (_firstBorder is Border.Top or Border.Bottom)
            {
                if (enemyPos.x < playerPos.x)
                {
                    enemy.movementDirection = Vector2.left;
                    return;
                }

                if (enemyPos.x >= playerPos.x)
                {
                    enemy.movementDirection = Vector2.right;
                }
            }
        }

        private bool HandleCorner(EnemyAbstract enemy, Vector2 position, Vector2 bounds)
        {
            var (close, far) = DistanceToBorder(position, bounds);

            if (_firstBorder is Border.Left && _secondBorder is Border.Top or Border.Bottom)
            {
                if (close.x < TilesToRunAway)
                {
                    enemy.movementDirection = Vector2.right;
                    _isEscapingCorner = true;

                    return true;
                }
            }

            if (_firstBorder is Border.Right && _secondBorder is Border.Top or Border.Bottom)
            {
                if (far.x < TilesToRunAway)
                {
                    enemy.movementDirection = Vector2.left;
                    _isEscapingCorner = true;

                    return true;
                }
            }

            if (_firstBorder is Border.Top && _secondBorder is Border.Left or Border.Right)
            {
                if (far.y < TilesToRunAway)
                {
                    enemy.movementDirection = Vector2.down;
                    _isEscapingCorner = true;

                    return true;
                }
            }

            if (_firstBorder is Border.Bottom && _secondBorder is Border.Left or Border.Right)
            {
                if (close.y < TilesToRunAway)
                {
                    enemy.movementDirection = Vector2.up;
                    _isEscapingCorner = true;

                    return true;
                }
            }

            _isEscapingCorner = false;
            return false;
        }

        private (Vector2 closeBorder, Vector2 farBorder) DistanceToBorder(Vector2 position, Vector2 bounds)
        {
            var closeBorders = new Vector2(position.x, position.y);
            var farBorders = new Vector2(bounds.x - position.x, bounds.y - position.y);

            return (closeBorders, farBorders);
        }

        private bool TooCloseToLeft(Vector2 position)
        {
            return position.x <= BorderPadding;
        }

        private bool TooCloseToRight(Vector2 position, Vector2 bounds)
        {
            return position.x >= bounds.x - BorderPadding;
        }

        private bool TooCloseToTop(Vector2 position, Vector2 bounds)
        {
            return position.y >= bounds.y - BorderPadding;
        }

        private bool TooCloseToBottom(Vector2 position)
        {
            return position.y <= BorderPadding;
        }

        private bool TryTeleport(EnemyAbstract enemy)
        {
            var chance = (float)enemy.EscapeCornerCounter / enemy.EscapeCornerMax;

            var roll = Random.value;

            Debug.Log($"escape counter: {enemy.EscapeCornerCounter}, max escapes: {enemy.EscapeCornerMax}");
            Debug.Log($"chance: {chance}, roll: {roll}");

            if (roll <= chance)
            {
                TeleportBehindPlayer(enemy);
                enemy.EscapeCornerCounter = 0;
                return true;
            }

            return false;
        }

        private void TeleportBehindPlayer(EnemyAbstract enemy)
        {
            Vector2 playerPos = enemy.Player.position;
            Vector2 enemyPos = enemy.transform.position;

            const float offset = 8f;

            var fromPlayerToEnemy = (enemyPos - playerPos).normalized;

            var target = playerPos - fromPlayerToEnemy * offset;

            enemy.transform.position = target;
        }
    }
}