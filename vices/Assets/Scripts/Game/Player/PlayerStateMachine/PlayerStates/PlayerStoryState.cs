using static PlayerStateManager;

public class PlayerStoryState : PlayerBaseState
{
    public PlayerStoryState(PlayerContext context, PlayerStateFactory factory) : base(context, factory) { }
    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.Set(PlayerControlls.IsMoving, 0);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if (!_context.CutsceneStarted)
        {
            player.SwitchState(_factory.IdleState());
        }
    }
}
