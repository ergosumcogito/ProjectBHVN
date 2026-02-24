using UnityEngine;

namespace Core.PlayerLogic.Abilities
{
    public class PlayerAbilitySlot : MonoBehaviour
    {
        private enum Trigger
        {
            FlashPressed,
            HealPressed
        }

        [SerializeField] private AbilitySO ability;
        [SerializeField] private Trigger trigger;
        [SerializeField] private bool provideDirection = true;

        private InputReader _inputReader;
        private Rigidbody2D _rb;
        private PlayerHealth _health;

        private Vector2 _lastDirection = Vector2.right;
        private float _nextUseTime;
        
        public float MaxHealth => _health.MaxHealth;

        private void Awake()
        {
            _rb = GetComponentInParent<Rigidbody2D>();
            _health = GetComponentInParent<PlayerHealth>();
        }

        public void SetInputReader(InputReader reader)
        {
            if (_inputReader && isActiveAndEnabled)
                Unsubscribe(_inputReader);

            _inputReader = reader;

            if (_inputReader && isActiveAndEnabled)
                Subscribe(_inputReader);
        }

        private void OnEnable()
        {
            if (_inputReader != null) Subscribe(_inputReader);
        }

        private void OnDisable()
        {
            if (_inputReader != null) Unsubscribe(_inputReader);
        }

        private void Subscribe(InputReader input)
        {
            switch (trigger)
            {
                case Trigger.FlashPressed: input.FlashPressed += TryUse; break;
                case Trigger.HealPressed: input.HealPressed += TryUse; break;
            }
        }

        private void Unsubscribe(InputReader input)
        {
            switch (trigger)
            {
                case Trigger.FlashPressed: input.FlashPressed -= TryUse; break;
                case Trigger.HealPressed: input.HealPressed -= TryUse; break;
            }
        }

        private void FixedUpdate()
        {
            if (!provideDirection || !_inputReader) return;

            var input = _inputReader.MovementInput;
            if (input.sqrMagnitude > 0.001f)
                _lastDirection = input.normalized;
        }

        private void TryUse()
        {
            if (ability == null) return;
            if (Time.time < _nextUseTime) return;

            var ctx = BuildContext();

            if (!ability.CanExecute(in ctx))
                return;

            ability.Execute(in ctx);
            _nextUseTime = Time.time + ability.Cooldown;
        }

        private AbilityContext BuildContext()
        {
            var dir = Vector2.zero;

            if (provideDirection)
            {
                if (_rb != null && _rb.linearVelocity.sqrMagnitude > 0.001f)
                    dir = _rb.linearVelocity.normalized;
                else if (_inputReader != null && _inputReader.MovementInput.sqrMagnitude > 0.001f)
                    dir = _inputReader.MovementInput.normalized;
                else
                    dir = _lastDirection;
            }

            return new AbilityContext
            {
                owner = gameObject,
                rb = _rb,
                health = _health,
                input = _inputReader,
                direction = dir,
                time = Time.time
            };
        }

        public float CooldownRemaining => Mathf.Max(0f, _nextUseTime - Time.time);
        public float CooldownDuration => ability != null ? ability.Cooldown : 0f;
        public AbilitySO Ability => ability;
    }
}