using UnityEngine;

namespace Core.Enemy_Logic
{
    /*
     * The StateManager is responsible for updating data of the current state
     */
    public class EnemyStateManager : MonoBehaviour
    {
        public EnemyAbstract enemy; // Reference to current enemy -> children of enemy

        public EnemyBaseState currentState;
        public EnemyBaseState EnemyAttackState = new EnemyAttackState();
        public EnemyBaseState EnemyChaseState = new EnemyChaseState();
        public EnemyBaseState EnemyDeathState = new EnemyDeathState();
        public EnemyBaseState EnemyIdleState = new EnemyIdleState();
        public EnemyBaseState EnemyFleeState = new EnemyFleeState();

        public void Start()
        {
            //starting state for the state machine--> enemy chases immediately after spawning
            enemy = gameObject.GetComponent<EnemyAbstract>(); // get the exact child element of enemy 
            currentState = EnemyChaseState;
            currentState.EnterState(this, enemy);
        }

        public void Update()
        {
            // will call any logic in Update State from the current state every frame
            currentState.UpdateState(this, enemy);
        }


        public void SwitchState(EnemyBaseState state)
        {
            currentState = state;
            state.EnterState(this, enemy);
        }
    }
}