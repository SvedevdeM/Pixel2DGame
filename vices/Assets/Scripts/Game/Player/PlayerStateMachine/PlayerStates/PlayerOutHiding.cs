using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStateManager;

public class PlayerOutHiding : PlayerBaseState
{
    public PlayerOutHiding(PlayerContext context, PlayerStateFactory factory) : base(context, factory) { }
    public override void EnterState(PlayerStateManager player)
    {
        player.Animator.Set(PlayerControlls.IsMoving, false);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        //if ()
    }
}
