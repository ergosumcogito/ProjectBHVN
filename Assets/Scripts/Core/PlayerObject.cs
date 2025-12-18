using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public GameObject playerPrefab;

    private const float MaxHealth = 100f;
    private float _health = MaxHealth;
    private bool _isDead = false;

    public bool IsDead
    {
        get => _isDead;
    }

    public float PlayerHealth
    {
        get => _health;
        set => _health = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0f)
        {
            Destroy(gameObject);
        }

        Debug.Log($"{name}: Damage taken" + _health + "Health");
    }

    private void Die()
    {
        Destroy(playerPrefab.gameObject, 1.5f);
        Debug.Log("Player died");
        _isDead = true;
        //make sure to add player reference in chase -> stop chase after player died
    }
}