using Core.Enemy_Logic;
using UnityEngine;

public class BibleProjectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private float critChance;
    [SerializeField] private float lifeTime = 5f; // default lifetime
    private float lifeTimer;
    private Transform player;
    public Vector3 rotationAxis = Vector3.up;
    
    public void Init(float speed, float damage, float attackSpeed, Vector3 position, Transform player)
    {
        this.speed = speed;
        this.damage = damage;

        lifeTimer = 0f; // reset timer on spawn
        lifeTime /= attackSpeed + 1;
        GetComponent<Transform>().position = position;
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
            Debug.Log("Turning");
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
