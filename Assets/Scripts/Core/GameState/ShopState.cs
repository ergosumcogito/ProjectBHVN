public class ShopState : GameState
{
    public ShopState(GameStateMachine machine) : base(machine) {}

    public override void Enter()
    {
        UIManager.Instance.ShowShop(true);
    }

    public override void Exit()
    {
        UIManager.Instance.ShowShop(false);
    }
}