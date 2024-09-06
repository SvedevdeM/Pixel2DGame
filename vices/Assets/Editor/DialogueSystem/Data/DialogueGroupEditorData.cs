using System;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor.Data
{
    [Serializable]
    public class DialogueGroupEditorData
    {
        [SerializeField] public string ID;
        [SerializeField] public string Name;
        [SerializeField] public Vector2 Position;
    }
}