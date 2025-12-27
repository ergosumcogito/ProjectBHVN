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
            if (other.CompareTag("Player"))
            {
                playerData.coins += _coinValue;
                Debug.Log("Coin Collected: "+ _coinValue + ". Player now has: " + playerData.coins + " Coins");
                Destroy(gameObject);
            }
        }

        public string GetDropType()
        {
            return "Coin";
        }
        
    }
}