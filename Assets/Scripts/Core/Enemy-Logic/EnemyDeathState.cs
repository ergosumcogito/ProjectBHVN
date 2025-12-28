using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyDeathState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager,EnemyAbstract enemy)
        { 
            Debug.Log("Enemy died");
            enemy.SetAnimationState(
                chasing: false,
                attacking: false,
                dead: true);
            enemy.MovementDirection = Vector2.zero;
            enemy.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            enemy.DestroyAfterDeath(1.27f);
            //GameObject.Destroy(enemy.gameObject,0.2f);
        }

        public override void UpdateState(EnemyStateManager manager,EnemyAbstract enemy)
        {
            // nothing
        }

        public override void OnCollisionEnter(EnemyStateManager manager,EnemyAbstract enemy)
        {
            // no reaction when dead
        }
    }
}