using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class Minotauros : EnemyAbstract
    {
        [SerializeField] private List<GameObject> drops;

        [Header("Minotauros Overrides")] [SerializeField]
        private float minotaurosMoveSpeed = 3f;

        [SerializeField] private float minotaurosAttackPower = 25f;
        [SerializeField] private float minotaurosMaxHealth = 70f;
        [SerializeField] private int minotaurosCoinMin = 10;
        [SerializeField] private int minotaurosCoinMax = 20;

        protected override void Awake()
        {
            MoveSpeed = minotaurosMoveSpeed;
            AttackPower = minotaurosAttackPower;
            MaxHealth = minotaurosMaxHealth;

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
                    coin.SetValue(Random.Range(minotaurosCoinMin, minotaurosCoinMax));
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