using UnityEngine;

namespace Core.PlayerLogic.Abilities
{
    public struct AbilityContext
    {
        public GameObject owner;
        public Rigidbody2D rb;
        public PlayerHealth health;
        public InputReader input;

        public Vector2 direction;
        public float time;
    }
}