using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
    private PlayerContext _playerContext;
    
    public PlayerStateFactory(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public PlayerBaseState StoryState() => new PlayerStoryState(_playerContext, this);
    public PlayerBaseState IdleState() => new PlayerIdleState(_playerContext, this);
    public PlayerBaseState WalkingState() => new PlayerMovingState(_playerContext, this);
    public PlayerBaseState RunningState() => new PlayerRunningState(_playerContext, this);
    public PlayerBaseState ToHidingState() => new PlayerToHidingState(_playerContext, this);
    public PlayerBaseState InHidingState() => new PlayerHidingState(_playerContext, this);
    public PlayerBaseState OutHidingState() => new PlayerOutHiding(_playerContext, this);
    public PlayerBaseState DamagedState() => new PlayerDamagedState(_playerContext, this);
    public PlayerBaseState DeathState() => new PlayerDeathState(_playerContext, this); 
}