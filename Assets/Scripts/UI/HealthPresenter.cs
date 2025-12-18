using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private PlayerHealth playerHealth;


    void Start()
    {
        healthBar.Init(playerHealth.MaxHealth);
        playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float newHealth)
    {
        healthBar.UpdateHealth(newHealth);
    }
}