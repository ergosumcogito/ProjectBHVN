using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyAttackState : EnemyBaseState
    {
        private PlayerHealth playerHealth;
        private float attackCooldown = 1.2f;
        private float lastAttackTime = 0f;
        public override void EnterState(EnemyStateManager manager,EnemyAbstract enemy)
        {
           //Debug.Log("Enemy entered Attack State");
           playerHealth = enemy.Player.GetComponent<PlayerHealth>();
        }

        public override void UpdateState(EnemyStateManager manager,EnemyAbstract enemy)
        {
            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }
            float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);

            if (distance > 1.5f)
            {
                manager.SwitchState(manager.EnemyChaseState);
                return;
            }

            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                playerHealth.TakeDamage(enemy.attackPower);
            }
        }
        

        public override void OnCollisionEnter(EnemyStateManager manager,EnemyAbstract enemy)
        {
            // not needed atm
        }
    }
}