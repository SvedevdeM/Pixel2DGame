using JabberwockyWorld.DialogueSystem.Scripts.Data;
using UnityEngine;
using System;

namespace JabberwockyWorld.DialogueSystem.Scripts
{
    [CreateAssetMenu(fileName = nameof(DialogueContainerInfo), menuName = "JabberwockyWorld/Dialogue/" + nameof(DialogueContainerInfo))]
    public class DialogueContainerInfo : ScriptableObject
    {
        private Action<DialogueContainerRuntimeData> _onDialogueContainerChange;

        public void ChangeDialogueContainer(DialogueContainerRuntimeData dialogue)
        {
            _onDialogueContainerChange?.Invoke(dialogue);
        }

        public void SubscribeOnDialogueContainerChange(Action<DialogueContainerRuntimeData> onChange)
        {
            _onDialogueContainerChange += onChange;
        }

        public void UnsubscribeOnDialogueContainerChange(Action<DialogueContainerRuntimeData> onChange)
        {
            _onDialogueContainerChange -= onChange;
        }
    }
}