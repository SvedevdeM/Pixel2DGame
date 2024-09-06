using UnityEngine;
using static PlayerStateManager;

public class PlayerDamagedState : PlayerBaseState
{
    private float _currentTime = 0.5f;
    public PlayerDamagedState(PlayerContext context, PlayerStateFactory factory) : base(context, factory) { }
    public override void EnterState(PlayerStateManager player)
    {
        _context.PlayerMovement.enabled = false;
        player.Animator.Set(PlayerControlls.IsMoving, 3);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime >= 0) return;
        _context.PlayerMovement.enabled = true;
        player.Animator.Set(PlayerControlls.IsHitted, false);
        player.SwitchState(_factory.IdleState());
    }
}
