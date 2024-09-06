using JabberwockyWorld.DialogueSystem.Scripts.Data;
using System.Collections;
using UnityEngine;
using Vices.Scripts.Core;

namespace JabberwockyWorld.DialogueSystem.Scripts
{
    public class DialogueManager : MonoBehaviour
    {
        private DialogueContainerInfo _dialogueContainer;
        private DialogueInfo _dialogueInfo;

        private DialogueContainerRuntimeData _currentDialogueContainer;
        private DialogueRuntimeData _currentDialogue;

        private bool _canSkipDialogue;

        private void Start()
        {
            _dialogueContainer = GameContext.Context.DialogueContainerInfo;
            _dialogueInfo = GameContext.Context.DialogueInfo;

            _dialogueContainer.SubscribeOnDialogueContainerChange(OnDialogueContainerChange);
            _dialogueInfo.SubscribeOnDialogueChange(OnDialogueChange);

            DontDestroyOnLoad(gameObject);
        }

        private void OnDialogueContainerChange(DialogueContainerRuntimeData dialogueContainer)
        {
            _currentDialogueContainer = dialogueContainer;

            _currentDialogue = GetStartDialogue(_currentDialogueContainer);
            _dialogueInfo.ChangeDialogue(_currentDialogue);

            _canSkipDialogue = false;

            StartCoroutine(Delay());
        }


        private void OnDialogueChange(DialogueRuntimeData dialogue)
        {
            _currentDialogue = dialogue;
        }

        private void Update()
        {
            if (_currentDialogueContainer == null) return;
            if (_currentDialogue == null) return;

            if (_currentDialogue.Type == DialogueType.Event)
            {
                _currentDialogue.Event.Execute();
                _currentDialogue = GetNextDialogue(_currentDialogue);
                _dialogueInfo.ChangeDialogue(_currentDialogue);
            }

            if (!Input.GetKeyDown(KeyCode.E)) return; //TODO: Change input system to a new one
            if (!_canSkipDialogue) return;

            if (_currentDialogue == null) return;

            if (_currentDialogue.Type == DialogueType.MultipleChoice)
            {
                _dialogueInfo.MultipleChoices(_currentDialogue);
                return;
            }

            switch (_currentDialogue.Type)
            {
                case DialogueType.MultipleChoice:
                    _dialogueInfo.MultipleChoices(_currentDialogue);
                    return;
                case DialogueType.Event:
                    _currentDialogue.Event.Execute();
                    return;
            }

            _currentDialogue = GetNextDialogue(_currentDialogue);
            _dialogueInfo.ChangeDialogue(_currentDialogue);
        }

        private DialogueRuntimeData GetStartDialogue(DialogueContainerRuntimeData dialogueContainer)
        {
            for (int i = 0; i < dialogueContainer.UngroupedDialogues.Count; i++)
            {
                var dialogue = dialogueContainer.UngroupedDialogues[i];
                if (dialogue.IsStartingDialogue)
                {
                    return dialogue;
                }
            }

            foreach (var element in dialogueContainer.Groups)
            {
                foreach (DialogueRuntimeData dialogue in element.Value)
                {
                    if (dialogue.IsStartingDialogue)
                    {
                        return dialogue;      
                    }
                }
            }

            return null;
        }

        private DialogueRuntimeData GetNextDialogue(DialogueRuntimeData dialogue)
        {
            return dialogue.Choices[0].NextDialogue;
        }

        private IEnumerator Delay()
        {
            yield return new WaitForEndOfFrame();

            _canSkipDialogue = true;
        }
    }
}
