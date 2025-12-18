using UnityEngine;

namespace Core.Enemy_Logic
{
    public class EnemyDeathState : EnemyBaseState
    {
        public override void EnterState(EnemyStateManager manager,EnemyAbstract enemy)
        {
          //  Debug.Log("Enemy died");
            enemy.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GameObject.Destroy(enemy.gameObject,0.1f);
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