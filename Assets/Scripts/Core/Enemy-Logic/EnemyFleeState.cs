using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyFleeState : EnemyBaseState
    {
        private enum FirstBorder
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        private static FirstBorder _firstBorder = FirstBorder.None;
        private static int _borderPadding = 3;

        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            Debug.Log("Entered Flee State");

            enemy.canMove = true;

            enemy.SetAnimationState(
                new AnimationStateChange(AnimationBool.IsChasing, true),
                new AnimationStateChange(AnimationBool.IsAttacking, false),
                new AnimationStateChange(AnimationBool.IsInactive, false),
                new AnimationStateChange(AnimationBool.IsDead, false),
                new AnimationStateChange(AnimationBool.IsIdle, false));
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

        private static void FleeFromPlayer(EnemyAbstract enemy)
        {
            Vector2 enemyPos = enemy.transform.position;
            Vector2 playerPos = enemy.Player.position;
            var levelBounds = enemy.LevelBounds;

            CheckFirstBorder(enemyPos, levelBounds);
            Debug.Log($"first border: {_firstBorder}");
            
            if (_firstBorder != FirstBorder.None)
            {
                HandleFirstBorder(enemy, enemyPos, playerPos, levelBounds);
                return;
            }

            var direction = (enemyPos - playerPos).normalized;
            enemy.movementDirection = direction;
        }

        private static void CheckFirstBorder(Vector2 position, Vector2 bounds)
        {
            if (position.x - _borderPadding <= 1)
            {
                _firstBorder = FirstBorder.Left;
                return;
            }

            if (position.x + _borderPadding >= bounds.x)
            {
                _firstBorder = FirstBorder.Right;
                return;
            }

            if (position.y + _borderPadding >= bounds.y)
            {
                _firstBorder = FirstBorder.Top;
                return;
            }

            if (position.y - _borderPadding <= 1)
            {
                _firstBorder = FirstBorder.Bottom;
                return;
            }
            
            _firstBorder = FirstBorder.None;
        }

        private static void HandleFirstBorder(EnemyAbstract enemy, Vector2 enemyPos, Vector2 playerPos, Vector2 bounds)
        {
            if (_firstBorder is FirstBorder.Left or FirstBorder.Right)
            {
                if (enemyPos.y < playerPos.y)
                {
                    enemy.movementDirection = Vector2.down;
                    return;
                }

                if (enemyPos.y > playerPos.y)
                {
                    enemy.movementDirection = Vector2.up;
                    return;
                }
            }

            if (_firstBorder is FirstBorder.Top or FirstBorder.Bottom)
            {
                if (enemyPos.x < playerPos.x)
                {
                    enemy.movementDirection = Vector2.left;
                    return;
                }

                if (enemyPos.x > playerPos.x)
                {
                    enemy.movementDirection = Vector2.right;
                }
            }
        }

        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
        }
    }
}