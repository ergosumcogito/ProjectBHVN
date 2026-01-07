using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyFleeState : EnemyBaseState
    {
        private static void GetBands(EnemyAbstract enemy, out float flee, out float idleMin, out float idleMax)
        {
            if (enemy is Necromancer necro)
            {
                flee = necro.FleeDistance;
                idleMin = necro.IdleMinDistance;
                idleMax = necro.IdleMaxDistance;
                
                enemy.SetAnimationState(
                    chasing: true,
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

            var d = Vector2.Distance(enemy.transform.position, enemy.Player.position);
            Debug.Log($"[FLEE] d={d:F2} dir={enemy.MovementDirection} enemy={enemy.name}");

            // leave flee when we're no longer too close
            if (d > flee)
            {
                if (d <= idleMax) manager.SwitchState(manager.EnemyIdleState);
                else manager.SwitchState(manager.EnemyChaseState);
                return;
            }

            Vector2 dirAway = (enemy.transform.position - enemy.Player.position).normalized;
            enemy.MovementDirection = dirAway;
        }

        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
        }
    }
}