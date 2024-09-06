using JabberwockyWorld.AnimationSystem.Scripts;
using System.Collections;
using UnityEngine;
using Vices.Scripts.Game;


public class EnemyRenderer : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private SpriteAnimator _customAnimator;

    private string _animatorState;
    private int _animationID;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        _enemy.SubscribeOnChangeDirectionChange(OnDirectionChanged);
        _enemy.SubscribeOnChangeStateChange(OnStateChanged);
    }

    private void OnStateChanged(EnemyStateType type)
    {
        _customAnimator.Set("Main", 0);
        switch (type)
        {
            case EnemyStateType.None: 
                break;
            case EnemyStateType.Attack:
                _customAnimator.Set("Main", 1);
                _animatorState = "Attack";
                break;
            case EnemyStateType.Idle:
                _customAnimator.Set("Main", 0);
                _animatorState = "Main";
                break;
            case EnemyStateType.LastRemembered:
                _customAnimator.Set("Main", 3);
                _animatorState = "Walk";
                break;
            case EnemyStateType.Search:
                _customAnimator.Set("Main", 3);
                _animatorState = "Walk";
                break;
            case EnemyStateType.Patrool:
                _customAnimator.Set("Main", 3);
                _animatorState = "Walk";
                break;
            case EnemyStateType.Rage:
                _customAnimator.Set("Main", 2);
                _animatorState = "Run";
                break;
            case EnemyStateType.Cooldown:
                _customAnimator.Set("Main", 0);
                _animatorState = "Main";
                break;
        }

        _customAnimator.Set(_animatorState, _animationID);
    }

    private void OnDirectionChanged(EnemyDirectionType type)
    {
        _customAnimator.Flip = false;

        switch (type)
        {
            case EnemyDirectionType.SideRight:
                _animationID = 2;
                _customAnimator.Flip = true;
                break;
            case EnemyDirectionType.SideLeft:
                _animationID = 2;
                break;
            case EnemyDirectionType.Down:
                _animationID = 0;
                break;
            case EnemyDirectionType.Up:
                _animationID = 1;
                break;
        }
        if (_animatorState == null) return;
        _customAnimator.Set(_animatorState, _animationID);
    }
}
