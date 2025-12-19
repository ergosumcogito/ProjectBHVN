using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Cyclops : EnemyAbstract
    {
        [SerializeField] private List<GameObject> drops;
        
        [Header("Cyclops Overrides")] 
        [SerializeField] private float cyclopsMoveSpeed = 4f;
        [SerializeField] private float cyclopsAttackPower = 50f;
        [SerializeField] private float cyclopsMaxHealth = 90f;
        [SerializeField] private int cyclopsCoinMin = 20;
        [SerializeField] private int cyclopsCoinMax = 50;
        protected override void Awake()
        { 
            MoveSpeed = cyclopsMoveSpeed;
            AttackPower = cyclopsAttackPower;
            MaxHealth = cyclopsMaxHealth;
            
            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }
        public override void Drop()
        {
            Debug.Log("Goblin DROP() START");
            if (drops.Count > 0)
            {
                var prefab = drops[Random.Range(0, drops.Count)];
                if (prefab.TryGetComponent<Coin>(out var coin))
                {
                    coin.SetValue(Random.Range(cyclopsCoinMin, cyclopsCoinMax));
                }

                Instantiate(prefab, transform.position, Quaternion.identity);

                //coin.SetValue(value);
            }
            else
            {
                Debug.Log("List was empty");
            }
        }
    }
    }
    
