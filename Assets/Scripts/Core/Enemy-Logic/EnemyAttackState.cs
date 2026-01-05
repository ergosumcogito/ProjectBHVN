using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyAttackState : EnemyBaseState
    {
        private PlayerHealth playerHealth;
        private float attackCooldown = 2.5f;
        private float lastAttackTime = 0f;
        public override void EnterState(EnemyStateManager manager,EnemyAbstract enemy)
        {
           Debug.Log("Enemy entered Attack State");
           playerHealth = enemy.Player.GetComponent<PlayerHealth>();
           enemy.canMove = false;  // stop movement
           enemy.SetAnimationState(
               chasing: false,
               attacking: true,
               dead: false);
           Debug.Log("IsAttacking SET TRUE");
           
           enemy.FlipWhileAttack();
           
        }


        public override void UpdateState(EnemyStateManager manager,EnemyAbstract enemy)
        {

            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }
            float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);

            if (distance > enemy.AttackRange)
            {
                enemy.canMove = true; // re-enable movement when chase starts
                manager.SwitchState(manager.EnemyChaseState);
                return;
            }

            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                playerHealth.TakeDamage(enemy.attackPower);
                enemy.FlipWhileAttack();
            }
        }
        

        public override void OnCollisionEnter(EnemyStateManager manager,EnemyAbstract enemy)
        {
            // not needed atm
        }
    }
}