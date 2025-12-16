using System;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Coin : MonoBehaviour, IDropable
    {
        [Header("Value for Coin")] protected int Value = 3; // value is being calculated by children class and set via setter
        
        //reference to PlayerData
        

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
                // TBD add money to player

                Destroy(gameObject);
            }
        }

        public string GetDropType()
        {
            return "Coin";
        }
        
    }
}