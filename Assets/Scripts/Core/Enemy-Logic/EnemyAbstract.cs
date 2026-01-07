using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Enemy_Logic
{
    [RequireComponent(typeof(BoxCollider2D))] // every game object with this script is required to have a box colider
    public abstract class EnemyAbstract : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        protected EnemyStateManager stateManager;
        protected Animator animator;
        public bool canMove = true;

        public Rigidbody2D rb;
        public Vector2 MovementDirection;

        [Header("Damage Flash when enemy gets nHit from Player")]
        public DamageFlash damageFlash;


        [Header("Flag for flipping")] public bool facingRight = true;


        // Stats must be implemented by the children classes - attributes loaded with default values
        [Header("Base Stats")] protected float MoveSpeed = -1f;
        protected float MaxHealth = -1f;
        protected float AttackPower = -1f;

        //Protected field should be visable for othe classes in the folder e.g. State Machine with States
        public float moveSpeed => MoveSpeed;
        public float maxHealth => MaxHealth;
        public float attackPower => AttackPower;

        [SerializeField] protected float attackRange = 3.5f;
        public float AttackRange => attackRange;

        [Header("References")] public Transform Player { get; protected set; } // is used by the Spawner

        protected float currentHealth;

        [FormerlySerializedAs("playerPlayer")] [FormerlySerializedAs("playerHealth")]
        public PlayerObject playerObjectPlayerObject;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>(); //new

            animator = GetComponent<Animator>();
            stateManager = GetComponent<EnemyStateManager>(); // get the current child instance of enemy
            currentHealth = MaxHealth;
            damageFlash = GetComponent<DamageFlash>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (animator == null)
            {
                Debug.LogError($"{name} has no Animator attached!");
            }

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

        public void SetAnimationState(bool chasing, bool attacking, bool dead)
        {
            animator.SetBool("IsChasing", chasing);
            animator.SetBool("IsAttacking", attacking);
            animator.SetBool("IsDead", dead);
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
            Debug.Log(" !!!health of enemy at beginning!!! :" + currentHealth);
        }


        protected virtual void Update()
        {
            stateManager?.Update();
            if (canMove && !IsDead)
            {
                FlipController();
            }
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
            damageFlash?.Flash();
            currentHealth -= amount;
            Debug.Log("Enemy took damage : " + currentHealth);
            if (currentHealth <= 0f)
            {
                Die();
            }
        }


        public void Die()
        {
            //PlayDeathAnimation();
            Debug.Log(name + " DIED!");
            Drop();
            stateManager?.SwitchState(stateManager.EnemyDeathState);
        }

        public bool IsDead => currentHealth <= 0f;

// delete ?
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerObject>();
                if (player == null) return;

                player.TakeDamage(AttackPower);
            }
        }

        void FixedUpdate()
        {
            if (!canMove) // do not move if in attackstate
                return;
            rb.MovePosition(rb.position + MovementDirection * moveSpeed * Time.fixedDeltaTime);
        }

        // For flipping enemy-------------------------------------------------
        protected void Flip()
        {
            Debug.Log("FLIP CALLED");

            facingRight = !facingRight;
            Vector3 scale = transform.localScale; // actual scalr of game object
            scale.x *= -1; // by multiplying x with -1 we rotate horizontally 
            transform.localScale = scale; // set the new scale
        }

        public void FlipWhileAttack()
        {
            float positionPlayer = Player.position.x;
            float positionEnemy = transform.position.x;
            float pos = positionEnemy - positionPlayer;
Debug.Log("Current position: "+ pos);
            if (pos <= 0 && !facingRight)
            {
                Flip();
            }

            else if (pos > 0 && facingRight)
            {
                Flip();
            }
        }

        public void FlipController()
        {
            if (MovementDirection.x > 0 && !facingRight)
                Flip();
            else if (MovementDirection.x < 0 && facingRight)
                Flip();
        }

        // End flipping Enemy--------------------------------------------------

        public abstract void Drop();

        //Following Functions are for delaying death state in order to play animation

        public void DestroyAfterDeath(float delay)
        {
            StartCoroutine(DestroyRoutine(delay));
        }

        public IEnumerator DestroyRoutine(float delay)
        {
            yield return
                new WaitForSeconds(delay); // stops method time -> delay after goes further and destroys enemy object
            Destroy(gameObject);
        }
    }
}