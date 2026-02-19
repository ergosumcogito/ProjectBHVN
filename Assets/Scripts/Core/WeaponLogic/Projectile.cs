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
        this.direction = dir.normalized;
        this.speed = speed;
        this.damage = damage;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAbstract enemy = collision.GetComponentInParent<EnemyAbstract>();
        if (enemy == null) return;
        if (!enemy.IsTargattable)
        {
            Destroy(gameObject);
            return;
        }

        enemy.TakeDamage(damage);
        Destroy(gameObject); // delete bullet
    }
}