using UnityEngine;

namespace Core.PlayerLogic.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Heal", fileName = "Heal")]
    public class HealAbility : AbilitySO
    {
        [SerializeField] private float healPercentage = 25f;

        public override bool CanExecute(in AbilityContext context)
        {
            return context.health.CurrentHealth < context.health.MaxHealth;
        }

        public override void Execute(in AbilityContext context)
        {
            var healAmount = context.health.MaxHealth * (healPercentage / 100f);
            context.health.Heal(healAmount);
        }
    }
}