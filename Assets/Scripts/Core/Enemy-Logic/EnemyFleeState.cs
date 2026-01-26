using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyFleeState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            enemy.MovementDirection = Vector2.zero;
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }

            var distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);

            if (distance > enemy.FleeDistance)
            {
                manager.SwitchState(manager.EnemyIdleState);
                return;
            }

            var direction = GetFleeDirection(enemy.transform.position, enemy.Player.position, enemy.LevelBounds);
            enemy.MovementDirection = direction;
        }

        private static Vector2 GetFleeDirection(Vector2 enemy, Vector2 player, Vector2 levelBounds)
        {
            if (enemy.x - 1f <= 0 || enemy.x + 2f >= levelBounds.x)
            {
                return enemy.y > player.y ? Vector2.up : Vector2.down;
            }

            if (enemy.y - 2f <= 0 || enemy.y + 3f >= levelBounds.y)
            {
                return enemy.x > player.x ? Vector2.right : Vector2.left;
            }

            return (enemy - player).normalized;
        }

        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
        }
    }
}