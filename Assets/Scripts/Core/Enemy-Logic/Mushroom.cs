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
            var prefab = drops[Random.Range(0, drops.Count)];
            if (prefab.TryGetComponent<Coin>(out var coin))
            {
                coin.SetValue(Random.Range(mushroomCoinMin, mushroomCoinMax));
            }

            Instantiate(prefab, transform.position, Quaternion.identity);

            //coin.SetValue(value);
        }
    }
}