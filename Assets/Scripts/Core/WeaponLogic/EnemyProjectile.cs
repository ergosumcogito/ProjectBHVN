using Core.Enemy_Logic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.WeaponLogic
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 20f;
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioSource source;
        [SerializeField] [Range(0f, 1f)] private float volume = 1f;
        [SerializeField] private Vector2 pitchRange = new(0.95f, 1.05f);

        private Rigidbody2D _rigidBody;
        private float _damage;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.gravityScale = 0f;
            _rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (!clip) return;
            if (!source) source = GetComponent<AudioSource>();
            if (!source) source = gameObject.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f;
            source.pitch = Random.Range(pitchRange.x, pitchRange.y);

            source.PlayOneShot(clip, volume);
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
            if (!other.CompareTag("PlayerHitbox")) return;

            // Debug.Log($"collision detected with: {other.name}, {other.tag}");

            var health = other.GetComponentInParent<PlayerHealth>();
            health.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}