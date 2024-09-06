using JabberwockyWorld.AnimationSystem.Scripts;
using UnityEngine;

public class PlayerContext
{
    public CharacterController CharacterController { get; private set; }
    public Transform Transform { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public HidingInBush HidingInBush { get; private set; }
    public SpriteAnimator Animator { get; private set; }
    public bool CutsceneStarted { get; set; }
    //private Input _playerInput;

    public PlayerContext(GameObject player)
    {
        CharacterController = player.GetComponent<CharacterController>();
        Transform = player.GetComponent<Transform>();
        PlayerMovement = player.GetComponent<PlayerMovement>();
        HidingInBush = player.GetComponent<HidingInBush>();
        Animator = player.GetComponent<SpriteAnimator>();
    }
}
