using JabberwockyWorld.AnimationSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Scripts;
using System;
using UnityEngine;

namespace Vices.Scripts.Game
{
    [Serializable]
    public class CutsceneKey
    {
        public CutsceneActionType Type;
        public string SceneName;
        public float Time;
        public float WaitTime;
        public int InputAmount;
        
        public DialogueContainerRuntimeData Dialogue;
        public AudioClip Audio;
        public EventData EventData;
        public CameraTarget CameraTarget;
        public MoveTarget MoveTarget;
        public CutsceneAnimation CutsceneAnimation;
        public GameObjectEnable GameObjectEnable;
    }

    [Serializable]
    public class CameraTarget
    {
        public Transform Transform;
        public float Duration;
    }

    [Serializable]
    public class MoveTarget
    {
        public Transform Target;
        public Transform Point;
        public float Duration;
    }

    [Serializable]
    public class GameObjectEnable
    {
        public GameObject GameObject;
        public bool Enable;
    }

    [Serializable]
    public class CutsceneAnimation
    {
        public GameObject Target;
        public SwitchNode Node;
        public string Drive;
        public int Value;
        public bool Flip;
    }
}