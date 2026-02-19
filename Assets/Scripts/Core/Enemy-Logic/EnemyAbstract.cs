using System.Collections.Generic;
using Core.WeaponLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Enemy_Logic
{
    // [RequireComponent(typeof(BoxCollider2D))] // every game object with this script is required to have a box colider
    public abstract class EnemyAbstract : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        protected EnemyStateManager stateManager;
        public Animator animator;
        public Rigidbody2D rb;
        private GameRoundManager _gameRoundManager;
        private Vector2 _levelBounds;

        private bool _isFleeingType;
        private bool _firesProjectiles;
        private GameObject _projectile;
        private float _projectileSpeed;
        public bool canMove = true;
        protected bool isTargettable;

        private int _splitLevel = 2;
        private bool _isSplitting = false;

        // Base Stats
        private float _maxHealth = 50f;
        private float _attackPower = 10f;
        private float _moveSpeed = 1f;
        private float _attackRange = 3.5f;
        private float _cooldown = 2f;
        private float _cooldownSpecial = 10f;
        private float _spawnFadeTime;

        private int _coinMin = 10;
        private int _coinMax = 20;

        private float _idleMinDistance;
        private float _idleMaxDistance;

        //only relevant for fleeing type enemies
        private bool _canTeleportBehindPlayer;
        private int _escapeCornerMax;
        private int _escapeCornerCounter;
        private int _escapeCornerSpeedMultiplier;

        private float _currentHealth;
        private float _timeSinceLastAttack;
        private float _timeSinceLastSpecialAttack;

        private GameObject _summons;
        private int _summonAmount;
        private float _summonRadius;

        private List<GameObject> _drops = new();

        private RigidbodyConstraints2D _constraints;

        public Vector2 movementDirection;
        public bool IsDead => _currentHealth <= 0f;

        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }


        public float AttackPower
        {
            get => _attackPower;
            set => _attackPower = value;
        }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public float AttackRange
        {
            get => _attackRange;
            set { _attackRange = value < 0f ? Mathf.Infinity : value; }
        }

        public float Cooldown
        {
            get => _cooldown;
            set => _cooldown = value;
        }

        public float CooldownSpecial
        {
            get => _cooldownSpecial;
            set => _cooldownSpecial = value;
        }

        public float SpawnFadeTime
        {
            get => _spawnFadeTime;
            set => _spawnFadeTime = value;
        }

        public float IdleMinDistance
        {
            get => _idleMinDistance;
            set => _idleMinDistance = value;
        }

        public float IdleMaxDistance
        {
            get => _idleMaxDistance;
            set => _idleMaxDistance = value;
        }

        public int CoinMin
        {
            get => _coinMin;
            set => _coinMin = value;
        }

        public int CoinMax
        {
            get => _coinMax;
            set => _coinMax = value;
        }

        public GameObject Summons
        {
            get => _summons;
            set => _summons = value;
        }

        public int SummonAmount
        {
            get => _summonAmount;
            set => _summonAmount = value;
        }

        public float SummonRadius
        {
            get => _summonRadius;
            set => _summonRadius = value;
        }

        public float TimeSinceLastAttack
        {
            get => _timeSinceLastAttack;
            set => _timeSinceLastAttack = value;
        }

        public float TimeSinceLastSpecialAttack
        {
            get => _timeSinceLastSpecialAttack;
            set => _timeSinceLastSpecialAttack = value;
        }

        protected List<GameObject> Drops
        {
            get => _drops;
            set => _drops = value;
        }

        public bool IsFleeingType
        {
            get => _isFleeingType;
            set => _isFleeingType = value;
        }

        public bool IsSplitting
        {
            get => _isSplitting;
            set => _isSplitting = value;
        }

        public bool FiresProjectiles
        {
            get => _firesProjectiles;
            set => _firesProjectiles = value;
        }

        public GameObject Projectile
        {
            get => _projectile;
            set => _projectile = value;
        }

        public float ProjectileSpeed
        {
            get => _projectileSpeed;
            set => _projectileSpeed = value;
        }

        public bool CanTeleportBehindPlayer
        {
            get => _canTeleportBehindPlayer;
            set => _canTeleportBehindPlayer = value;
        }

        public int EscapeCornerMax
        {
            get => _escapeCornerMax;
            set => _escapeCornerMax = value;
        }

        public int EscapeCornerCounter
        {
            get => _escapeCornerCounter;
            set => _escapeCornerCounter = value;
        }

        public int EscapeCornerSpeedMultiplier
        {
            get => _escapeCornerSpeedMultiplier;
            set => _escapeCornerSpeedMultiplier = value;
        }

        public Vector2 LevelBounds
        {
            get => _levelBounds;
            set => _levelBounds = value;
        }

        [Header("Damage Flash when enemy gets nHit from Player")]
        public DamageFlash damageFlash;

        [Header("Flag for flipping")] public bool facingRight = true;

        [Header("References")] public Transform Player { get; protected set; } // is used by the Spawner

        public SpriteRenderer SpriteRenderer
        {
            get => spriteRenderer;
            set => spriteRenderer = value;
        }

        public bool IsTargattable
        {
            get => isTargettable;
            set => isTargettable = value;
        }

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>(); 
            animator = GetComponent<Animator>();
            stateManager = GetComponent<EnemyStateManager>(); // get the current child instance of enemy
            _currentHealth = MaxHealth;
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
            // Debug.Log("!!!health of enemy at beginning!!! :" + _currentHealth);
        }

        protected virtual void Update()
        {
            if (!_gameRoundManager)
            {
                _gameRoundManager = FindFirstObjectByType<GameRoundManager>();
                LevelBounds = _gameRoundManager.GetCurrentLevelBounds();
            }

            if (canMove && !IsDead)
            {
                FlipController();
            }
        }

        public void SetAnimationState(params AnimationStateChange[] stateChanges)
        {
            foreach (var change in stateChanges)
            {
                animator.SetBool(change.state.GetAnimatorName(), change.value);
            }
        }

        public void Init(Transform player)
        {
            Player = player;
        }

        public void TakeDamage(float amount)
        {
            damageFlash?.Flash();
            _currentHealth -= amount;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<PlayerObject>();
            if (player == null) return;

            player.TakeDamage(AttackPower);
        }

        void FixedUpdate()
        {
            if (!canMove) // do not move if in attackstate
                return;
            rb.MovePosition(rb.position + movementDirection * (_moveSpeed * Time.fixedDeltaTime));
        }

        // For flipping enemy-------------------------------------------------
        protected void Flip()
        {
            // Debug.Log("FLIP CALLED");

            facingRight = !facingRight;
            Vector3 scale = transform.localScale; // actual scalr of game object
            scale.x *= -1; // by multiplying x with -1 we rotate horizontally 
            transform.localScale = scale; // set the new scale
        }

        public void FlipController()
        {
            if (movementDirection.x > 0 && !facingRight)
                Flip();
            else if (movementDirection.x < 0 && facingRight)
                Flip();
        }

        private void Drop()
        {
            // Debug.Log("Goblin DROP() START");
            if (Drops.Count > 0)
            {
                var prefab = Drops[Random.Range(0, Drops.Count)];
                if (!prefab.TryGetComponent<Coin>(out _)) return;
                var coinPrefab = Instantiate(prefab, transform.position, Quaternion.identity);

                var comp = coinPrefab.GetComponent<Coin>();
                comp.CoinValue = Random.Range(CoinMin, CoinMax + 1);
            }
            else
            {
                Debug.Log("List was empty");
            }
        }

        public void FreezeEnemy()
        {
            _constraints = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }

        public void UnfreezeEnemy()
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Methods Used for indicating enemy prepares for shooting projectile and Reset

        public void TurnRed()
        {
            spriteRenderer.color = new Color(1f, 0f, 0f);
        }

        public void TurnWhite()
        {
            spriteRenderer.color = new Color(1f, 1f, 1f);
        }

        public void Attack()
        {
            TimeSinceLastAttack = Time.time;
            // FlipWhileAttack();
            PerformAttack();
        }

        protected virtual void PerformAttack()
        {
            if (FiresProjectiles)
            {
                var dir = (Player.position - transform.position).normalized;
                var spawnPos = transform.position + dir * 0.5f;

                var go = Instantiate(Projectile, spawnPos, Quaternion.identity);

                var projectile = go.GetComponent<EnemyProjectile>();
                projectile.Init(dir, ProjectileSpeed, AttackPower);

                Debug.Log("Fired Projectile!");
                return;
            }

            var player = Player.GetComponent<PlayerHealth>();
            player.TakeDamage(AttackPower);

            Debug.Log("has attacked!");
        }

        protected void SwitchEnemyState(EnemyState state)
        {
            if (state == EnemyState.Idle)
            {
                stateManager.SwitchState(stateManager.EnemyIdleState);
            }
            else if (state == EnemyState.Attack)
            {
                stateManager.SwitchState(stateManager.EnemyAttackState);
            }
            else if (state == EnemyState.Chase)
            {
                stateManager.SwitchState(stateManager.EnemyChaseState);
            }
        }

        protected enum EnemyState
        {
            Idle,
            Attack,
            Chase
        }

        protected void DestroySelf()
        {
            Drop();

            Destroy(gameObject);
        }
    }
}