using UnityEngine;

namespace Core.PlayerLogic.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Heal", fileName = "Heal")]
    public class HealAbility : AbilitySO
    {
        [SerializeField] private float healAmount = 25f;

        public override bool CanExecute(in AbilityContext context)
        {
            return context.health.CurrentHealth < context.health.MaxHealth;
        }

        public override void Execute(in AbilityContext context)
        {
            context.health.Heal(healAmount);
        }
    }
}