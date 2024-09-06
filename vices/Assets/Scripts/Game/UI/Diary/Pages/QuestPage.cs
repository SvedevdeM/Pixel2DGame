using JabberwockyWorld.Quest.Scripts;
using TMPro;
using UnityEngine;

namespace Vices.Scripts
{
    public class QuestPage : DiaryPage
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _mainPoint;

        [SerializeField] private QuestInfo _questInfo;
        [SerializeField] private QuestUI[] _questUI;

        protected override void OpenPage()
        {
            ClosePage();

            for (int i = 0; i < _questInfo.Quests.Count; i++)
            {
                _questUI[i].Show(_questInfo.Quests[i]);
            }        
        }

        protected override void ClosePage()
        {
            _name.text = "Quest Name";
            _description.text = "";
            _mainPoint.text = "";

            for (int i = 0; i < _questUI.Length; i++)
            {
                _questUI[i].Hide();
            }
        }
    }
}
