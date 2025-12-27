using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Mushroom : EnemyAbstract
    {
        [SerializeField] private List<GameObject> drops;

        [Header("mushroom Overrides")] [SerializeField]
        private float mushroomMoveSpeed = 3f;

        [SerializeField] private float mushroomAttackPower = 25f;
        [SerializeField] private float mushroomMaxHealth = 70f;
        [SerializeField] private int mushroomCoinMin = 10;
        [SerializeField] private int mushroomCoinMax = 20;

        protected override void Awake()
        {
            MoveSpeed = mushroomMoveSpeed;
            AttackPower = mushroomAttackPower;
            MaxHealth = mushroomMaxHealth;

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
                    comp.CoinValue = Random.Range(mushroomCoinMin, mushroomCoinMax + 1);
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