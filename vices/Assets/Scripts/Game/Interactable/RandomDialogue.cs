using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Vices.Scripts
{
    public class RandomDialogue : Interactable
    {
        [SerializeField] private DialogueContainerInfo _dialogueContainerInfo;
        [SerializeField] private List<DialogueContainerRuntimeData> _dialogues;
        protected override void OnEnter(Collider other)
        {
            Debug.Log("ON_ENTER");
            OnInteract();
        }

        protected override void OnInteract()
        {
            Debug.Log("ASDFSGFGH");
            int randomIndex = Random.Range(0, _dialogues.Count);
            _dialogueContainerInfo.ChangeDialogueContainer(_dialogues[randomIndex]);
        }
    }
}