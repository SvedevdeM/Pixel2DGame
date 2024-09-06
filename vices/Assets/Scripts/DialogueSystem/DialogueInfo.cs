using UnityEngine;
using System;

namespace JabberwockyWorld.DialogueSystem.Scripts.Data
{
    [CreateAssetMenu(fileName = nameof(DialogueInfo), menuName = "JabberwockyWorld/Dialogue/" + nameof(DialogueInfo))]
    public class DialogueInfo : ScriptableObject
    {
        private Action<DialogueRuntimeData> _onDialogueChange;
        private Action<DialogueRuntimeData> _onMultipleChoices;

        public void ChangeDialogue(DialogueRuntimeData dialogue)
        {
            _onDialogueChange?.Invoke(dialogue);
        }       

        public void SubscribeOnDialogueChange(Action<DialogueRuntimeData> onChange)
        {
            _onDialogueChange += onChange;
        }

        public void UnsubscribeOnDialogueChange(Action<DialogueRuntimeData> onChange)
        {
            _onDialogueChange -= onChange;
        }

        public void MultipleChoices(DialogueRuntimeData dialogue)
        {
            _onMultipleChoices?.Invoke(dialogue);
        }

        public void SubscribeOnMultipleChoices(Action<DialogueRuntimeData> onChange)
        {
            _onMultipleChoices += onChange;
        }

        public void UnsubscribeOnMultipleChoices(Action<DialogueRuntimeData> onChange)
        {
            _onMultipleChoices -= onChange;
        }
    }
}