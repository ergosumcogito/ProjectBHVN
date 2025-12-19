using System;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Coin : MonoBehaviour, IDropable
    {
        [Header("Value for Coin")] protected int Value = 3; // value is being calculated by children class and set via setter
        
        public PlayerData playerData;
        

        //Protected field should be visable for othe classes in the folder
        public int value => Value;

        public void SetValue(int val)
        {
            Value = val;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerData.coins += Value;
                Debug.Log("Coin Collected: "+ Value + ". Player now has: " + playerData.coins + " Coins");
                Destroy(gameObject);
            }
        }

        public string GetDropType()
        {
            return "Coin";
        }
        
    }
}