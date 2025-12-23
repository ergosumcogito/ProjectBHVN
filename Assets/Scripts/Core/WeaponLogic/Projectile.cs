using Core.Enemy_Logic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private float critChance;
    private Vector3 direction;

    public void Init(Vector3 dir, float speed, float damage)
    {
        this.direction = dir;
        this.speed = speed;
        this.damage = damage;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAbstract enemy = collision.GetComponent<EnemyAbstract>();
        if (enemy == null) return;
        
        enemy.TakeDamage(damage);
       // Destroy(gameObject); // TODO remove this when enemy.Die works properly
    }

}