using UnityEngine;

public class GameplayState : GameState
{
    private RoundSystem _round;

    public GameplayState(GameStateMachine machine, RoundSystem round)
        : base(machine)
    {
        _round = round;
    }

    public override void Enter()
    {
        UIManager.Instance.ShowGameplayHUD(true);

        RoundEvents.OnRoundSurvived += HandleRoundEnd;
        RoundEvents.OnRoundFailed   += HandleRoundFailed;

        _round.StartRound();
    }

    public override void Exit()
    {
        UIManager.Instance.ShowGameplayHUD(false);

        RoundEvents.OnRoundSurvived -= HandleRoundEnd;
        RoundEvents.OnRoundFailed   -= HandleRoundFailed;
    }

    private void HandleRoundEnd()
    {
        machine.ChangeState(GameStateType.Shop);
    }

    private void HandleRoundFailed()
    {
        machine.ChangeState(GameStateType.GameOver);
    }
}