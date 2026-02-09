using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core.Enemy_Logic
{
    public class Coin : MonoBehaviour, IDropable
    {
        [Header("Value for Coin")] private int _coinValue; // value is being calculated by children class and set via setter
        public PlayerData playerData;
        

        //Protected field should be visable for othe classes in the folder
        public int CoinValue
        {
            get => _coinValue;
            set => _coinValue = value;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // must be a player
            if (!other.CompareTag("Player"))
                return;
            
            // must have runtime currency component
            if (!other.TryGetComponent(out PlayerRuntimeCurrency currency))
                return;
            
            currency.AddCoins(_coinValue);
            
          //  Debug.Log($"Coin Collected: {_coinValue}. Player now has: {currency.Coins} Coins");
            
            Destroy(gameObject);
        }

        public string GetDropType()
        {
            return "Coin";
        }
        
    }
}