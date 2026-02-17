using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyChaseState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            Debug.Log("Switched to Chase State");

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
            //Debug.Log("DISTANCE TO PLAYER: " + distance);
            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }
            
            if (enemy.IsFleeingType)
            {
                if (Time.time - enemy.TimeSinceLastAttack > enemy.Cooldown)
                {
                    manager.SwitchState(manager.EnemyAttackState);
                    return;
                }
            }

            var distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);
            
            if (enemy.IsFleeingType)
            {
                if (distance < enemy.IdleMaxDistance)
                {
                    manager.SwitchState(manager.EnemyIdleState);
                }

                ChasePlayer(enemy);
                return;
            }
            
            if (distance <= enemy.AttackRange && Time.time - enemy.TimeSinceLastAttack > enemy.Cooldown)
            {
                manager.SwitchState(manager.EnemyAttackState);
                return;
            }

            if (distance <= enemy.AttackRange && Time.time - enemy.TimeSinceLastAttack <= enemy.Cooldown)
            {
                manager.SwitchState(manager.EnemyIdleState);
                return;
            }

            ChasePlayer(enemy);
        }

        private static void ChasePlayer(EnemyAbstract enemy)
        {
            Vector2 direction = (enemy.Player.position - enemy.transform.position).normalized;
            enemy.movementDirection = direction;
        }
    }
}