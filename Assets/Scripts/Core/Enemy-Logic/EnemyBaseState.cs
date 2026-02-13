using UnityEngine;

namespace Core.Enemy_Logic
{
    /*
     * The abstract Enemy BaseState basically serves as abstract State of the specified States --> here : Chase, Attack, Death
     */
    public abstract class EnemyBaseState
    {
        public abstract void EnterState(EnemyStateManager manager, EnemyAbstract enemy);
        public abstract void UpdateState(EnemyStateManager manager,EnemyAbstract enemy);
        public abstract void OnCollisionEnter(EnemyStateManager manager,EnemyAbstract enemy, Collision2D collision); 
    }
}
