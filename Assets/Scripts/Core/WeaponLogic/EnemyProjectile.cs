using Core.Enemy_Logic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.WeaponLogic
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 20f;

        private Rigidbody2D _rigidBody;
        private float _damage;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.gravityScale = 0f;
            _rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        public void Init(Vector3 dir, float speed, float damage)
        {
            _damage = damage;

            dir = dir.normalized;
            _rigidBody.linearVelocity = dir * speed;

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"collision detected with: {other.name}, {other.tag}");

            var health = other.GetComponentInParent<PlayerHealth>();
            health.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}