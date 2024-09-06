using System;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor.Data
{
    [Serializable]
    public class DialogueChoiceEditorData
    {
        [SerializeField] public string Text;
        [SerializeField] public string NodeID;
    }
}