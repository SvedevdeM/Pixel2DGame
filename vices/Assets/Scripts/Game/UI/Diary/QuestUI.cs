using JabberwockyWorld.Quest.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vices.Scripts
{
    public class QuestUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _mainPoint;

        private QuestData _quest;

        public void OnPointerClick(PointerEventData eventData)
        {
            _name.text = _quest.Name;
            _description.text = _quest.Description;
            _mainPoint.text = _quest.MainPoint;
        }

        public void Show(QuestData quest)
        {
            gameObject.SetActive(true);
            _quest = quest;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}