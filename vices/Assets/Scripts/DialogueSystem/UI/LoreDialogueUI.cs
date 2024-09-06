using JabberwockyWorld.DialogueSystem.Scripts.Data;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vices.Scripts.Core;

namespace Vices.Scripts.Game
{
    public class LoreDialogueUI : MonoBehaviour
    {
        [SerializeField] private DialogueInfo _dialogueInfo;
        [SerializeField] private LocalizationInfo _localization;
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private GameObject _blackLines;
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _text;

        [SerializeField] private float _typingSpeed = 0.05f;

        private void Start()
        {
            HideDialogue();
            _dialogueInfo.SubscribeOnDialogueChange(ChangeText);
        }

        private void ChangeText(DialogueRuntimeData dialogue)
        {
            if (dialogue == null)
            {
                HideDialogue();
                return;
            }

            if (!_localization.TryGetLocalization(dialogue.Name, out var text))
            {
                text = dialogue.Text;
            }

            _dialoguePanel.SetActive(true);
            _blackLines.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(TypeText(text));

            if (dialogue.Actor == null) return;
            
            _name.text = dialogue.Actor.Name;
            if(dialogue.Actor.Icon != null) _avatar.sprite = dialogue.Actor.Icon;
        }

        private void OnDestroy()
        {
            _dialogueInfo.UnsubscribeOnDialogueChange(ChangeText);
        }

        private void HideDialogue()
        {
            _name.text = "";
            _text.text = "";
            _avatar.sprite = null;

            _dialoguePanel.SetActive(false);
            _blackLines.SetActive(false);
        }

        private IEnumerator TypeText(string textToType)
        {
            _text.text = "";
            foreach (char letter in textToType.ToCharArray())
            {
                _text.text += letter;
                yield return new WaitForSeconds(_typingSpeed);
            }
        }
    }
}