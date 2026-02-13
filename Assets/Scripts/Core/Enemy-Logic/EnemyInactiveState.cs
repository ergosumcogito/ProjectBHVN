using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyInactiveState : EnemyBaseState
    {
        private SpriteRenderer _spriteRenderer;
        private Color _c;

        private float _fadeElapsed;
        private bool _fullOpacity;

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
            _fadeElapsed = 0f;
            _fullOpacity = false;
            
            _spriteRenderer.color = new Color(_c.r, _c.g, _c.b, 0f);
            enemy.IsTargattable = false;
        }

        public override void UpdateState(EnemyStateManager manager, EnemyAbstract enemy)
        {
            if (!_fullOpacity)
            {
                FadeIn(enemy, enemy.SpawnFadeTime);
                return;
            }

            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
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
                {
                    Debug.Log("too far");
                    manager.SwitchState(manager.EnemyChaseState);
                    return;
                }
                
                manager.SwitchState(manager.EnemyIdleState);
                return;
            }

            if (distance > enemy.AttackRange)
            {
                manager.SwitchState(manager.EnemyChaseState);
            }
            else
            {
                manager.SwitchState(manager.EnemyIdleState);
            }
        }


        public override void OnCollisionEnter(EnemyStateManager manager, EnemyAbstract enemy)
        {
        }

        private void FadeIn(EnemyAbstract enemy, float duration)
        {
            _fadeElapsed += Time.deltaTime;
            
            var t = (duration <= 0f) ? 1f : Mathf.Clamp01(_fadeElapsed / duration);
            var a = Mathf.Lerp(0f, 1f, t);
            
            _spriteRenderer.color = new Color(_c.r, _c.g, _c.b, a);

            if (t >= 1f)
            {
                _fullOpacity = true;
                enemy.IsTargattable = true;
            }
        }
    }
}