using System;
using UnityEngine;

namespace Core
{
    public class PlayerHealth : MonoBehaviour
    {
        public PlayerData playerData;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => playerData.maxHealth;
        public event Action<float> OnHealthChanged;
        public event Action OnPlayerDied;

        
        void Start()
        {
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth);
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
            Debug.Log("Damage has been taken Amount: " + amount + " Health left: " + CurrentHealth);
            
            OnHealthChanged?.Invoke(CurrentHealth);
            
            if (CurrentHealth <= 0f)
            {
                OnPlayerDied?.Invoke(); // other systems know that the Player is dead
            }
        }
    }
}