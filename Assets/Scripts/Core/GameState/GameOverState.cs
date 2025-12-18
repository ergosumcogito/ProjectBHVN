using UnityEngine;

public class GameOverState : GameState
{
    public GameOverState(GameStateMachine machine) : base(machine) {}

    public override void Enter()
    {
        UIManager.Instance.ShowGameplayHUD(false);
        UIManager.Instance.ShowShop(false);
        UIManager.Instance.ShowMainMenu(false);
        UIManager.Instance.ShowGameOver(true);
    }

    public override void Exit()
    {
        UIManager.Instance.ShowGameOver(false);
    }
}