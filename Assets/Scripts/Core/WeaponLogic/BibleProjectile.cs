using Core.Enemy_Logic;
using UnityEngine;

public class BibleProjectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private float critChance;
    [SerializeField] private float lifeTime = 5f; // default lifetime
    [SerializeField] private float duration = 1f;
    private float lifeTimer;
    private PlayerRuntimeStats _playerRuntimeStats;
    private Transform player;
    private Vector3 rotationAxis = Vector3.forward;
    
    public void Init(float speed, float damage, float attackSpeed, Transform player, float newDuration)
    {
        this.speed = speed;
        this.damage = damage;
        lifeTimer = 0f; // reset timer on spawn
        duration = newDuration;
        lifeTime = lifeTime / attackSpeed + duration;   
        this.player = player;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
        if (player != null)
        {
            transform.RotateAround(player.position, rotationAxis, speed * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAbstract enemy = collision.GetComponentInParent<EnemyAbstract>();
        if (enemy == null) return;

        enemy.TakeDamage(damage);
    }
    
}
