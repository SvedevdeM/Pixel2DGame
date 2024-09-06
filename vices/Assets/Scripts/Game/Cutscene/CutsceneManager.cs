using JabberwockyWorld.AnimationSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vices.Scripts.Core;
using Vices.Scripts.Game.UI;

namespace Vices.Scripts.Game
{
    public class CutsceneManager : MonoBehaviour
    {
        private DialogueInfo _dialogueInfo;
        private DialogueContainerInfo _dialogueContainerInfo;
        private CutsceneInfo _info;
        private CutsceneTimeline _timeline;

        private List<IExecutable> _executableEvents;
        private List<CutsceneKey> _cutsceneKeys;

        private float _currentTime;
        private int _inputAmount;
        private bool _isSkippable = true;
        private bool _blockTime = false;

        private void Start()
        {
            _info = GameContext.Context.CutsceneInfo;
            _dialogueInfo = GameContext.Context.DialogueInfo;
            _dialogueContainerInfo = GameContext.Context.DialogueContainerInfo;

            _executableEvents = new List<IExecutable>();
            _cutsceneKeys = new List<CutsceneKey>();

            _info.SubscribeOnCutsceneStart(StartCutscene);
        }

        private void StartCutscene(CutsceneTimeline timeline, bool isSkippable)
        {
            FindFirstObjectByType<PlayerStateManager>().StartCutscene();

            _timeline = timeline;
            _isSkippable = isSkippable;

            _cutsceneKeys.Clear();
            _cutsceneKeys.AddRange(timeline.Keys);

            _currentTime = 0;
        }

        private void Update()
        {
            if (_timeline == null) return;

            for (int i = 0; i < _executableEvents.Count; i++)
            {
                _executableEvents[i].Execute();
            }

            if (_blockTime) return;
            _currentTime += Time.deltaTime;
            for (int i = 0; i < _cutsceneKeys.Count; i++)
            {
                if (_cutsceneKeys[i].Time > _currentTime) continue;

                InvokeKey(_cutsceneKeys[i]);
                _cutsceneKeys.RemoveAt(i);
            }
        }

        private void InvokeKey(CutsceneKey key)
        {
            switch (key.Type)
            {
                case CutsceneActionType.None:
                    break;
                case CutsceneActionType.TargetMove:
                    _executableEvents.Add(new TargetMoveAction(key.MoveTarget, RemoveExecutable));
                    break;
                case CutsceneActionType.CameraMove:
                    _executableEvents.Add(new CameraMoveAction(key.CameraTarget, RemoveExecutable));
                    break;
                case CutsceneActionType.PlayDialogue:
                    _dialogueContainerInfo.ChangeDialogueContainer(key.Dialogue);
                    break;
                case CutsceneActionType.PlayAnimation:
                    var animationData = key.CutsceneAnimation;
                    if (!animationData.Target.TryGetComponentInObject<SpriteAnimator>(out var animator))
                    {
                        animator = animationData.Target.AddComponent<SpriteAnimator>();
                    }

                    animator.Root = animationData.Node;
                    animator.Set(animationData.Drive, animationData.Value);
                    animator.Flip = animationData.Flip;
                    break;
                case CutsceneActionType.PlaySound:
                    break;
                case CutsceneActionType.WaitForInput:
                    _blockTime = true;
                    _inputAmount = key.InputAmount;
                    _dialogueInfo.SubscribeOnDialogueChange(WaitForInput);
                    break;
                case CutsceneActionType.EnableGameObject:
                    key.GameObjectEnable.GameObject.SetActive(key.GameObjectEnable.Enable);
                    break;
                case CutsceneActionType.ExecuteEvent:
                    key.EventData.Execute();
                    break;
                case CutsceneActionType.LoadScene:
                    SceneSystem.Singleton.LoadScene(key.SceneName);
                    break;
                case CutsceneActionType.EndCutscene:
                    var stateManager = FindAnyObjectByType<PlayerStateManager>();
                    key.CameraTarget.Transform = stateManager.transform;
                    stateManager.RestartPlayer();
                    stateManager.EndCutscene();
                    _executableEvents.Add(new CameraMoveAction(key.CameraTarget, RemoveExecutable));
                    break;
                case CutsceneActionType.ReturnToPlayer:
                    key.CameraTarget.Transform = FindAnyObjectByType<PlayerMovement>().transform;
                    _executableEvents.Add(new CameraMoveAction(key.CameraTarget, RemoveExecutable));
                    break;
                case CutsceneActionType.MovePlayer:                
                    key.MoveTarget.Target = FindObjectOfType<PlayerMovement>().transform;
                    _executableEvents.Add(new TargetMoveAction(key.MoveTarget, RemoveExecutable));                    
                    break;
                case CutsceneActionType.PlayPlayerAnimation:
                    var playerAnimationData = key.CutsceneAnimation;
                    playerAnimationData.Target.TryGetComponentInObject<SpriteAnimator>(out var playerAnimator);
                    playerAnimator.Root = playerAnimationData.Node;
                    playerAnimator.Set(playerAnimationData.Drive, playerAnimationData.Value);
                    playerAnimator.Flip = playerAnimationData.Flip;
                    break;
                case CutsceneActionType.BlackScreenShow:
                    BlackScreen.Screen.Show(() => Debug.Log("Screen Showed"));
                    break;
                case CutsceneActionType.BlackScreenHide:
                    BlackScreen.Screen.Hide(() => Debug.Log("Screen Hided"));
                    break;
                default:
                    break;
            }
        }

        private void RemoveExecutable(IExecutable executable)
        {
            _executableEvents?.Remove(executable);
        }

        private void WaitForInput(DialogueRuntimeData dialogue)
        {
            _inputAmount--;

            if (_inputAmount > 0) return;

            _blockTime = false;
            _dialogueInfo.UnsubscribeOnDialogueChange(WaitForInput);
        }
    }

    public class CameraMoveAction : IExecutable
    {
        private Transform _cameraFollowTarget;
        private Vector3 _targetPosition;
        private Vector3 _startPosition;
        private float _time = 0;
        private float _duration;

        private Action<IExecutable> _onEnd;

        public CameraMoveAction(CameraTarget target, Action<IExecutable> onEnd) 
        {
            //TODO: Change this code in future. Maybe save in context
            _cameraFollowTarget = GameObject.FindObjectOfType<CameraTargetFollow>().transform;
            _startPosition = _cameraFollowTarget.position;
            _targetPosition = target.Transform.position;
            _duration = target.Duration;

            _onEnd = onEnd;
        }

        public void Execute()
        {
            _time += Time.deltaTime;
            _cameraFollowTarget.position = Vector3.Lerp(_startPosition, _targetPosition, _time / _duration);

            if (_time < _duration) return;
 
            _cameraFollowTarget.position = _targetPosition;
            _onEnd?.Invoke(this);
        }
    }

    public class TargetMoveAction : IExecutable
    {
        private Transform _target;
        private Vector3 _targetPosition;
        private Vector3 _startPosition;

        private float _time = 0;
        private float _duration;

        private Action<IExecutable> _onEnd;

        public TargetMoveAction(MoveTarget target, Action<IExecutable> onEnd)
        {
            //TODO: Change this code in future. Maybe save in context

            _target = target.Target;
            _startPosition = _target.position;
            _targetPosition = target.Point.position;
            _duration = target.Duration;

            _onEnd = onEnd;
        }

        public void Execute()
        {
            _time += Time.deltaTime;
            _target.position = Vector3.Lerp(_startPosition, _targetPosition, _time / _duration);

            if (_time < _duration) return;

            _target.position = _targetPosition;
            _onEnd?.Invoke(this);
        }
    }
}
