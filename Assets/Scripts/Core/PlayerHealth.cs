using System;
using Core.Enemy_Logic;
using UnityEngine;

namespace Core
{
    public class PlayerHealth : MonoBehaviour
    {
        public PlayerRuntimeStats runtimeStats;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => runtimeStats.MaxHealth;
        public event Action<float> OnHealthChanged;
        public event Action OnPlayerDied;
        
        private DamageFlash damageFlash;

        void Awake()
        {
            damageFlash = GetComponent<DamageFlash>();
        }
        
        void Start()
        {
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth);
        }

        public void TakeDamage(float amount)
        {
            if (CurrentHealth <= 0f) return; // if player already died

            
            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
            Debug.Log("Damage has been taken Amount: " + amount + " Health left: " + CurrentHealth);
            
            damageFlash?.Flash();
            
            OnHealthChanged?.Invoke(CurrentHealth);
            
            if (CurrentHealth <= 0f)
            {
                OnPlayerDied?.Invoke(); // other systems know that the Player is dead
            }
        }
    }
}