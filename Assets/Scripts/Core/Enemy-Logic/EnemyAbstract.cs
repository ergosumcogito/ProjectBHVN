using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Enemy_Logic
{
    [RequireComponent(typeof(BoxCollider2D))] // every game object with this script is required to have a box colider
    public abstract class EnemyAbstract : MonoBehaviour
    {
        protected EnemyStateManager stateManager;

        // new code
        public Rigidbody2D rb;
        public Vector2 MovementDirection;

        // Stats must be implemented by the children classes - attributes loaded with default values
        [Header("Base Stats")] protected float MoveSpeed = -1f;
        protected float MaxHealth = -1f;
        protected float AttackPower = -1f;

        //Protected field should be visable for othe classes in the folder e.g. State Machine with States
        public float moveSpeed => MoveSpeed;
        public float maxHealth => MaxHealth;
        public float attackPower => AttackPower;


        [Header("References")] public Transform Player { get; protected set; } // is used by the Spawner

        protected float currentHealth;

        [FormerlySerializedAs("playerPlayer")] [FormerlySerializedAs("playerHealth")]
        public PlayerObject playerObjectPlayerObject;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>(); //new
            stateManager = GetComponent<EnemyStateManager>(); // get the current child instance of enemy
            currentHealth = MaxHealth;

            // Null-check => Ensures this GameObject has EnemyStateManager attached in Unity

            if (stateManager == null)
            {
                Debug.LogError($"{name} has no EnemyStateManager attached!");
            }

            // Check if base stats are set in children classes

            if (MaxHealth <= 0 || MoveSpeed <= 0 || AttackPower <= 0)
            {
                Debug.LogWarning(
                    $"{name} has uninitialized base stats!" +
                    $"[MaxHealth={MaxHealth}, MoveSpeed={MoveSpeed},AttackPower={AttackPower}]" +
                    $"Check child class!");
            }
        }


        protected virtual void Start()
        {
            // Player tagged in Unity as 'Player' -> find automatically the player if tagged
            if (Player == null)
            {
                Player = GameObject.FindWithTag("Player")?.transform;
            }

            if (Player == null)
            {
                Debug.LogError($"{name}: No Player found in scene! Make sure the Player has been tagged");
            }

            Init(Player);
        }


        protected virtual void Update()
        {
            stateManager?.Update();
        }


        public virtual void Init(Transform player)
        {
            Player = player;
        }

/*
 * TakeDamage is not a state itself but contributes to change of state gradually, therefore inside the Die() Method
 * The DeathState is called
 */
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            Drop();
            stateManager?.SwitchState(stateManager.EnemyDeathState);
        }

        public bool IsDead => currentHealth <= 0f;


        public virtual void OnCollisionEnter2d(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                playerObjectPlayerObject = collision.gameObject.GetComponent<PlayerObject>();
                playerObjectPlayerObject.TakeDamage(AttackPower);
            }
        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + MovementDirection * moveSpeed * Time.fixedDeltaTime);
        }

        public abstract void Drop();
    }
}