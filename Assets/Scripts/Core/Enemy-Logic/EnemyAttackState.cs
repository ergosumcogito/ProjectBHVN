using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyAttackState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            Debug.Log("Enemy entered Attack State");

            enemy.FreezeEnemy();

            enemy.SetAnimationState(
                new AnimationStateChange(AnimationBool.IsChasing, false),
                new AnimationStateChange(AnimationBool.IsAttacking, true),
                new AnimationStateChange(AnimationBool.IsInactive, false),
                new AnimationStateChange(AnimationBool.IsDead, false),
                new AnimationStateChange(AnimationBool.IsIdle, false));

            enemy.TimeSinceLastAttack = Time.time;
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                // return;
            }

            // var distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);
            //
            // if (distance > enemy.AttackRange)
            // {
            //     manager.SwitchState(manager.EnemyChaseState);
            // }
        }
    }
}