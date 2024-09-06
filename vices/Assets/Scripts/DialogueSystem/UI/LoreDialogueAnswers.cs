using JabberwockyWorld.DialogueSystem.Scripts.Data;
using UnityEngine;
using Vices.Scripts.Core;

namespace Vices.Scripts.Game
{
    public class LoreDialogueAnswers : MonoBehaviour
    {
        [SerializeField] private DialogueInfo _dialogueInfo;
        [SerializeField] private LocalizationInfo _localizationInfo;
        [SerializeField] private DialogueAnswerUI[] _answers;

        private void Start()
        {
            _dialogueInfo.SubscribeOnMultipleChoices(OnMultipleChoices);
            _dialogueInfo.SubscribeOnDialogueChange(HideAnswers);
        }

        private void OnMultipleChoices(DialogueRuntimeData dialogue)
        {
            for (int i = 0; i < dialogue.Choices.Count; i++)
            {
                if (_localizationInfo.TryGetLocalization(dialogue.Choices[i].Text, out string answer)) _answers[i].Show(dialogue.Choices[i], answer);
            }
        }

        private void HideAnswers(DialogueRuntimeData dialogue)
        {
            for (int i = 0; i < _answers.Length; i++)
            {
                _answers[i].Hide();
            }
        }

        private void OnDestroy()
        {
            _dialogueInfo.UnsubscribeOnMultipleChoices(OnMultipleChoices);
            _dialogueInfo.UnsubscribeOnDialogueChange(HideAnswers);
        }
    }
}