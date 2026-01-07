using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyIdleState : EnemyBaseState
    {
        private static void GetBands(EnemyAbstract enemy, out float flee, out float idleMin, out float idleMax)
        {
            if (enemy is Necromancer necro)
            {
                flee = necro.FleeDistance;
                idleMin = necro.IdleMinDistance;
                idleMax = necro.IdleMaxDistance;

                enemy.SetAnimationState(
                    chasing: false,
                    attacking: false,
                    dead: false);

                return;
            }

            flee = 5f;
            idleMin = 8f;
            idleMax = 13f;
        }

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

            GetBands(enemy, out float flee, out _, out float idleMax);

            float d = Vector2.Distance(enemy.transform.position, enemy.Player.position);

            if (d <= flee)
            {
                manager.SwitchState(manager.EnemyFleeState);
                return;
            }

            if (d > idleMax)
            {
                manager.SwitchState(manager.EnemyChaseState);
                return;
            }

            // stay idle in the band
            enemy.MovementDirection = Vector2.zero;
        }

        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
        }
    }
}