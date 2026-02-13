using System;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyIdleState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            Debug.Log("Switched to Idle State");

            enemy.SetAnimationState(
                new AnimationStateChange(AnimationBool.IsChasing, false),
                new AnimationStateChange(AnimationBool.IsAttacking, false),
                new AnimationStateChange(AnimationBool.IsInactive, false),
                new AnimationStateChange(AnimationBool.IsDead, false),
                new AnimationStateChange(AnimationBool.IsIdle, true));

            enemy.canMove = false;
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }
            
            // Debug.Log($"fleeing type: {enemy.IsFleeingType}");
            // Debug.Log($"ready to attack: {Time.time - enemy.TimeSinceLastAttack > enemy.CoolDown}");
            
            if (enemy.IsFleeingType)
            {
                if (Time.time - enemy.TimeSinceLastAttack > enemy.CoolDown)
                {
                    manager.SwitchState(manager.EnemyAttackState);
                    return;
                }
            }

            var distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);
            
            if (enemy.IsFleeingType)
            {
                if (distance < enemy.IdleMinDistance)
                {
                    Debug.Log("too close");
                    manager.SwitchState(manager.EnemyFleeState);
                    return;
                }

                if (distance > enemy.IdleMaxDistance)
                {Debug.Log("too far");
                    manager.SwitchState(manager.EnemyChaseState);
                }

                return;
            }


            if (distance > enemy.AttackRange)
            {
                manager.SwitchState(manager.EnemyChaseState);
                return;
            }

            if (distance <= enemy.AttackRange && Time.time - enemy.TimeSinceLastAttack > enemy.CoolDown)
            {
                manager.SwitchState(manager.EnemyAttackState);
            }
        }

        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
            //tbd
        }
    }
}