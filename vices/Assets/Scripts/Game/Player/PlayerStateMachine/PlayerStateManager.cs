using JabberwockyWorld.AnimationSystem.Scripts;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerContext Context { get; private set; } 
    public PlayerStateFactory StateFactory { get; private set; }
    public bool IsDead { get; private set; }

    public HealthInfo _health;
    public SwitchNode Animation; 
    public SpriteAnimator Animator; 

    public static class PlayerControlls
    {
        public const string IsHiding = "isHiding";
        public const string IsGoingHide = "isGoingHide";
        public const string IsMoving = "isMoving";
        public const string ShouldFlip = "shouldFlip";
        public const string Side = "side";
        public const string IsHitted = "Hitted";
        public const string IsDead = "Death";
    } 

    private void Awake()
    {
        Context = new PlayerContext(gameObject);
        StateFactory = new PlayerStateFactory(Context);

        CurrentState = StateFactory.IdleState();

        CurrentState.EnterState(this);
    }

    private void Start()
    {
        _health.SubscribeHealthChanged(OnDamaged);
        _health.SubscribeOnDeath(DeathLogic);
    }

    void Update()
    {
        CurrentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState playerState)
    {
        CurrentState = playerState;
        playerState.EnterState(this);
    }

    public void StartCutscene()
    {
        Context.CutsceneStarted = true;
        GetComponent<PlayerMovement>().enabled = false;
        CurrentState = StateFactory.StoryState();
        CurrentState.EnterState(this);
    }

    public void EndCutscene()
    {
        Context.CutsceneStarted = false;
    }

    private void OnDamaged(float currentHealth, float maxHealth)
    {
        if (IsDead) return;
        SwitchState(StateFactory.DamagedState());
    }

    private void DeathLogic()
    {
        IsDead = true;
        _health.UnsubscribeHealthChanged(OnDamaged);
        _health.UnsubscribeOnDeath(DeathLogic);
        SwitchState(StateFactory.DeathState());
    }

    public void RestartPlayer()
    {
        GetComponent<PlayerMovement>().enabled = true;
        Animator.Root = Animation;
        CurrentState = StateFactory.IdleState();
        CurrentState.EnterState(this);
    }
}
