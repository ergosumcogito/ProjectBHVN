using UnityEngine;

namespace Core.PlayerLogic.Abilities
{
    public abstract class AbilitySO : ScriptableObject
    {
        [SerializeField] private float cooldown = 1f;
        public float Cooldown => cooldown;
        
        public virtual bool CanExecute(in AbilityContext context) => true;

        public abstract void Execute(in AbilityContext context);
    }
}