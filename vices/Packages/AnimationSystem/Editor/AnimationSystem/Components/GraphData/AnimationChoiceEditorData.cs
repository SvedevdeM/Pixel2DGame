using System;
using UnityEngine;
using JabberwockyWorld.AnimationSystem.Scripts;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    [Serializable]
    public class AnimationChoiceEditorData
    {
        [SerializeField] public string Text;
        [SerializeField] public string NodeID;
    }
}
