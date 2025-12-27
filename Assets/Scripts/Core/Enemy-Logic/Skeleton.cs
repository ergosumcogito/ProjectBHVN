using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Skeleton : EnemyAbstract
    {
        [SerializeField] private List<GameObject> drops;
        
        [Header("skeleton Overrides")] 
        [SerializeField] private float skeletonMoveSpeed = 4f;
        [SerializeField] private float skeletonAttackPower = 50f;
        [SerializeField] private float skeletonMaxHealth = 90f;
        [SerializeField] private int skeletonCoinMin = 20;
        [SerializeField] private int skeletonCoinMax = 50;
        protected override void Awake()
        { 
            MoveSpeed = skeletonMoveSpeed;
            AttackPower = skeletonAttackPower;
            MaxHealth = skeletonMaxHealth;
            
            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }
        public override void Drop()
        {
            Debug.Log("skeleton DROP() START");
            if (drops.Count > 0)
            {
                var prefab = drops[Random.Range(0, drops.Count)];
                if (prefab.TryGetComponent<Coin>(out var component))
                {
                    var coinPrefab = Instantiate(prefab, transform.position, Quaternion.identity);

                    Coin comp = coinPrefab.GetComponent<Coin>();
                    comp.CoinValue = Random.Range(skeletonCoinMin, skeletonCoinMax + 1);
                }

                //coin.SetValue(value);
            }
            else
            {
                Debug.Log("List was empty");
            }
        }
    }
    }
    
