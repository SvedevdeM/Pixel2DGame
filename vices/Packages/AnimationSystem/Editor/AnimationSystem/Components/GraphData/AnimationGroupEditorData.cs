using System;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    [Serializable]
    public class AnimationGroupEditorData
    {
        [SerializeField] public string ID;
        [SerializeField] public string Name;
        [SerializeField] public Vector2 Position;
    }
}
