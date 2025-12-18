using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyChaseState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager,EnemyAbstract enemy)
        {
         // Debug.Log("Enemy started chasing the player");  
        }

        public override void UpdateState(EnemyStateManager manager,EnemyAbstract enemy)
        {
            float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);
            if (enemy.IsDead)
            {
                manager.SwitchState(manager.EnemyDeathState);
                return;
            }

            if (distance <= 1.5f)
            {
                manager.SwitchState(manager.EnemyAttackState);
                return;
            }
            // Calculate the direction from the enemy to the player
            Vector2 direction= (enemy.Player.position - enemy.transform.position).normalized;
            // Move the enemy toward the player
            //enemy.transform.position += (Vector3)(direction * enemy.moveSpeed * Time.deltaTime);
            //new approach
            enemy.MovementDirection = direction; 
        }

        public override void OnCollisionEnter(EnemyStateManager manager,EnemyAbstract enemy)
        {
            // can be implemented further if extra reaction to hitting walls for example
            
        }
    }
}