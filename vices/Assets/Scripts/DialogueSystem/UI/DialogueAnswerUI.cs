using JabberwockyWorld.DialogueSystem.Scripts.Data;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vices.Scripts.Game
{
    [RequireComponent(typeof(Button))]
    public class DialogueAnswerUI : MonoBehaviour
    {
        [SerializeField] private DialogueInfo _dialogueInfo;
        [SerializeField] private TMP_Text _text;
        
        private Button _button;
        private DialogueRuntimeData _nextDialogue;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonPressed);
            Hide();
        }

        public void Show(DialogueChoiceRuntimeData choiceData, string text)
        {
            _nextDialogue = choiceData.NextDialogue;
            _text.text = text;

            gameObject.SetActive(true);
        }

        private void OnButtonPressed()
        {
            _dialogueInfo.ChangeDialogue(_nextDialogue);
        }

        public void Hide()
        {
            _text.text = "";
            _nextDialogue = null;

            gameObject.SetActive(false);
        }
    }
}