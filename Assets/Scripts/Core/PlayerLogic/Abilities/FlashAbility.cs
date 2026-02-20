using UnityEngine;

namespace Core.PlayerLogic.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Flash", fileName = "Flash")]
    public class FlashAbility : AbilitySO
    {
        [SerializeField] private float flashDistance = 4f;
        [SerializeField] private bool stopBeforeObstacle = true;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float skin = 0.1f;

        public override bool CanExecute(in AbilityContext context)
        {
            return context.rb != null && context.direction.sqrMagnitude > 0.0001f;
        }

        public override void Execute(in AbilityContext context)
        {
            var rb = context.rb;
            var direction = context.direction.normalized;

            var start = rb.position;
            var target = start + direction * flashDistance;

            if (stopBeforeObstacle)
            {
                var hit = Physics2D.Raycast(start, direction, flashDistance, obstacleMask);
                if (hit.collider != null)
                    target = hit.point - direction * skin;
            }

            rb.MovePosition(target);
        }
    }
}