using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyInactiveState : EnemyBaseState
    {
        private SpriteRenderer _spriteRenderer;
        private Color _c;

        public override void EnterState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            enemy.SetAnimationState(
                new AnimationStateChange(AnimationBool.IsChasing, false),
                new AnimationStateChange(AnimationBool.IsAttacking, false),
                new AnimationStateChange(AnimationBool.IsInactive, true),
                new AnimationStateChange(AnimationBool.IsDead, false),
                new AnimationStateChange(AnimationBool.IsIdle, false));
            Debug.Log("Switched to Inactive State");

            _spriteRenderer = enemy.SpriteRenderer;

            _c = _spriteRenderer.color;
            _c.a = 0f;
            _spriteRenderer.color = _c;
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            if (!_fullOpacity)
            {
                FadeIn(1f, enemy);
                return;
            }

            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }

            float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);

            if (distance > enemy.AttackRange)
            {
                manager.SwitchState(manager.EnemyChaseState);
            }
        }


        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
        }

        private bool _fullOpacity;

        private void FadeIn(float target, EnemyAbstract enemy)
        {
            Color c = _spriteRenderer.color;

            if (c.a >= target)
            {
                _fullOpacity = true;
                enemy.IsTargattable = true;
                return;
            }

            c.a += enemy.SpawnSpeed;
            //Debug.Log(c.a);
            _spriteRenderer.color = c;
        }
    }
}