using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerContext _context;
    protected PlayerStateFactory _factory;

    public PlayerBaseState (PlayerContext context, PlayerStateFactory factory)
    {
        _context = context;
        _factory = factory;
    }
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);

    public MovingSide GetMovingSide()
    {
        if (_context.PlayerMovement._movement.y == 1 && _context.PlayerMovement._movement.x == 0) return MovingSide.Up;
        else if (_context.PlayerMovement._movement.y == -1 && _context.PlayerMovement._movement.x == 0) return MovingSide.Down;
        else
        {
            _context.Animator.Flip = _context.PlayerMovement._movement.x < 0;
            return MovingSide.Side;
        }
    }
}
