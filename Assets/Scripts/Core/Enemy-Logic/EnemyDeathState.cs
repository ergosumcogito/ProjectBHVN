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
            DisableCollider(enemy);
            Debug.Log("Switched to Death State");
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            // nothing
        }

        private static void DisableCollider(EnemyAbstract enemy)
        {
            var capsuleCollider2D = enemy.GetComponent<CapsuleCollider2D>();
            var boxCollider2D = enemy.GetComponentInChildren<BoxCollider2D>();
            capsuleCollider2D.enabled = false;
            boxCollider2D.enabled = false;
        }
    }
}