using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStateManager;

public class PlayerMovingState : PlayerBaseState
{
    public PlayerMovingState(PlayerContext context, PlayerStateFactory factory) : base(context, factory) { }
    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.Set(PlayerControlls.IsMoving, 1);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        _context.Animator.Set(PlayerControlls.Side, ((int)GetMovingSide()));
        CheckStates(player);
    }

    void CheckStates (PlayerStateManager player)
    {
        if (_context.PlayerMovement._isRunning)
            player.SwitchState(_factory.RunningState());
        else if ((_context.PlayerMovement._movement.x == 0) && (_context.PlayerMovement._movement.y == 0))
        {
            player.SwitchState(_factory.IdleState());
        }
        else if ((_context.HidingInBush.IsInHide))
        {
            player.SwitchState(_factory.ToHidingState());
        }
    }
}
