using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor.Data
{
    [Serializable]
    public class DialogueNodeEditorData
    {
        [SerializeField] public string ID;
        [SerializeField] public string Name;
        [SerializeField] public string Text;
        [SerializeField] public ActorInfo Actor;
        [SerializeField] public EventData Event;
        [SerializeField] public List<DialogueChoiceEditorData> Choices;
        [SerializeField] public string GroupID;
        [SerializeField] public DialogueType Type;
        [SerializeField] public Vector2 Position;
    }
}
