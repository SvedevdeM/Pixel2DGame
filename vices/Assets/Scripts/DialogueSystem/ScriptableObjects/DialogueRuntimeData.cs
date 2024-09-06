using JabberwockyWorld.DynamicEvents.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Scripts.Data
{
    public class DialogueRuntimeData : ScriptableObject
    {
        [field: SerializeField] public string Name;
        [field: SerializeField][field: TextArea()] public string Text;
        [field: SerializeField] public ActorInfo Actor;
        [field: SerializeField] public EventData Event;
        [field: SerializeField] public List<DialogueChoiceRuntimeData> Choices;
        [field: SerializeField] public DialogueType Type;
        [field: SerializeField] public bool IsStartingDialogue;

        public void Initialize(string name, string text, List<DialogueChoiceRuntimeData> choices, DialogueType type, bool isStartingDialogue, ActorInfo actorInfo = null, EventData eventInfo = null)
        {
            Name = name;
            Text = text;
            Actor = actorInfo;
            Event = eventInfo;
            Choices = choices;
            Type = type;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}
