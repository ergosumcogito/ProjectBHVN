using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Oculus : EnemyAbstract
    {
        //[Header("Coin")] [SerializeField] GameObject coinPrefab;
        [SerializeField] private List<GameObject> drops = new List<GameObject>();


        [Header("Goblin Overrides")] [SerializeField]
        private float oculusMoveSpeed = 1f;

        [SerializeField] private float oculusAttackPower = 20f;
        [SerializeField] private float oculusMaxHealth = 50f;
        [SerializeField] private int oculusCoinMin = 3;
        [SerializeField] private int oculusCoinMax = 7;

        protected override void Awake()
        {
            MoveSpeed = oculusMoveSpeed;
            AttackPower = oculusAttackPower;
            MaxHealth = oculusMaxHealth;

            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }

        public override void Drop()
        {
            Debug.Log("Oculus DROP() START");
            if (drops.Count > 0)
            {
                var prefab = drops[Random.Range(0, drops.Count)];
                if (prefab.TryGetComponent<Coin>(out var component))
                {
                    var coinPrefab = Instantiate(prefab, transform.position, Quaternion.identity);

                    Coin comp = coinPrefab.GetComponent<Coin>();
                    comp.CoinValue = Random.Range(oculusCoinMin, oculusCoinMax + 1);
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
