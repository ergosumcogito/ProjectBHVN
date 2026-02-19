using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyDeathState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            enemy.IsTargattable = false;
            enemy.FreezeEnemy();
            enemy.SetAnimationState(
                new AnimationStateChange(AnimationBool.IsChasing, false),
                new AnimationStateChange(AnimationBool.IsAttacking, false),
                new AnimationStateChange(AnimationBool.IsInactive, false),
                new AnimationStateChange(AnimationBool.IsDead, true),
                new AnimationStateChange(AnimationBool.IsIdle, false));
            
            Debug.Log("Switched to Death State");
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            // nothing
        }
    }
}