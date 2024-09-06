using JabberwockyWorld.Quest.Scripts;
using TMPro;
using UnityEngine;

namespace Vices.Scripts.Game
{
    public class TestQuestUI : MonoBehaviour
    {
        [SerializeField] private QuestInfo _questInfo;
        [SerializeField] private GameObject _questObjest;
        [SerializeField] private TMP_Text _questName;
        [SerializeField] private TMP_Text _questDescription;

        private void Start()
        {
            HideQuestInfo();
            _questInfo.SubscribeOnQuestAdd(ShowQuestInfo);
        }

        private void HideQuestInfo()
        {
            _questObjest.SetActive(false);
        }

        private void ShowQuestInfo(QuestData quest)
        {
            _questObjest.SetActive(true);
            _questName.text = quest.Name;
            _questDescription.text = quest.Description;
        }
    }
}