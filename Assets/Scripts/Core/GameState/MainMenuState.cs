public class MainMenuState : GameState
{
    public MainMenuState(GameStateMachine machine) : base(machine) {}

    public override void Enter()
    {
        UIManager.Instance.ShowMainMenu(true);
    }

    public override void Exit()
    {
        UIManager.Instance.ShowMainMenu(false);
    }
}