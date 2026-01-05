using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Goblin : EnemyAbstract
    {
        //[Header("Coin")] [SerializeField] GameObject coinPrefab;
        [SerializeField] private List<GameObject> drops = new List<GameObject>();


        [Header("Goblin Overrides")] [SerializeField]
        private float goblinMoveSpeed = 1f;

        [SerializeField] private float goblinAttackPower = 10f;
        [SerializeField] private float goblinMaxHealth = 50f;
        [SerializeField] private int goblinCoinMin = 3;
        [SerializeField] private int goblinCoinMax = 7;

        protected override void Awake()
        {
            MoveSpeed = goblinMoveSpeed;
            AttackPower = goblinAttackPower;
            MaxHealth = goblinMaxHealth;

            base.Awake(); // currentHealth already declared in the EnemyAbstract
        }

        public override void Drop()
        {
            Debug.Log("Goblin DROP() START");
            if (drops.Count > 0)
            {
                var prefab = drops[Random.Range(0, drops.Count)];
                if (prefab.TryGetComponent<Coin>(out var component))
                {
                    var coinPrefab = Instantiate(prefab, transform.position, Quaternion.identity);

                    Coin comp = coinPrefab.GetComponent<Coin>();
                    comp.CoinValue = Random.Range(goblinCoinMin, goblinCoinMax + 1);
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