using UnityEngine;
using Vices.Scripts.Game.UI;
using static PlayerStateManager;

public class PlayerDeathState : PlayerBaseState
{
    private float _currentTime = 1f;
    private bool _isBlackScreenShowed;
    public PlayerDeathState(PlayerContext context, PlayerStateFactory factory) : base(context, factory) { }
    public override void EnterState(PlayerStateManager player)
    {
        _context.PlayerMovement.enabled = false;
        player.Animator.Set(PlayerControlls.IsMoving, 4);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime >= 0) return;
        if (_isBlackScreenShowed) return;
        BlackScreen.Screen.ShowDeathScreen(() => BlackScreen.Screen.Hide());

        _isBlackScreenShowed = true;
    }
}