using System;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Core.Enemy_Logic
{
    public class Coin : MonoBehaviour, IDropable
    {
        [Header("Value for Coin")]
        private int _coinValue; // value is being calculated by children class and set via setter

        public PlayerData playerData;

        // Sounds for Coin Pickup
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioSource source;
        [SerializeField] [Range(0f, 1f)] private float volume = 1f;
        [SerializeField] private Vector2 pitchRange = new(0.95f, 1.05f);

        //Protected field should be visable for othe classes in the folder

        private void Awake()
        {
            if (!clip) return;

            if (!source) source = GetComponent<AudioSource>();
            if (!source) source = gameObject.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f;
        }

        public int CoinValue
        {
            get => _coinValue;
            set => _coinValue = value;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // must be a player
            if (!other.CompareTag("PlayerHitbox"))
                return;

            // must have runtime currency component
            // if (!other.TryGetComponent(out PlayerRuntimeCurrency currency))
            // return;
            if (!other.GetComponentInParent<PlayerRuntimeCurrency>()) return;

            var currency = other.GetComponentInParent<PlayerRuntimeCurrency>();
            currency.AddCoins(_coinValue);

            if (clip)
            {
                source.pitch = Random.Range(pitchRange.x, pitchRange.y);
                source.PlayOneShot(clip, volume);
            }

            Destroy(gameObject, clip.length);
        }

        public string GetDropType()
        {
            return "Coin";
        }
    }
}