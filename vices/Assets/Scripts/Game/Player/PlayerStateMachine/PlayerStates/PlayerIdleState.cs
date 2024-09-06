using JabberwockyWorld.AnimationSystem.Scripts;
using System.Collections;
using System.Collections.Generic;
using static PlayerStateManager;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerContext context, PlayerStateFactory factory) : base(context, factory) { }
    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.Set(PlayerControlls.IsMoving, 0);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if ((_context.PlayerMovement._movement.x != 0) || (_context.PlayerMovement._movement.y != 0))
        {
            if (_context.PlayerMovement._isRunning)
                player.SwitchState(_factory.RunningState());
            else
                player.SwitchState(_factory.WalkingState());
        }
    }
}
