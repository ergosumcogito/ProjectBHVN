public abstract class GameState
{
    protected GameStateMachine machine;

    protected GameState(GameStateMachine machine)
    {
        this.machine = machine;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update() {}
}