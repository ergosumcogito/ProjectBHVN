using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyDeathState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            enemy.IsTargattable = false;
            enemy.canMove = false;
            enemy.SetAnimationState(
                new AnimationStateChange(AnimationBool.IsChasing, false),
                new AnimationStateChange(AnimationBool.IsAttacking, false),
                new AnimationStateChange(AnimationBool.IsInactive, false),
                new AnimationStateChange(AnimationBool.IsDead, true),
                new AnimationStateChange(AnimationBool.IsIdle, false));

            enemy.movementDirection = Vector2.zero;
            Debug.Log("Switched to Death State");
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            // nothing
        }

        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
            // no reaction when dead
        }
    }
}