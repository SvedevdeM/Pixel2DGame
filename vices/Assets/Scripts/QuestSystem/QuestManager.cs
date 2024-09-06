using System.Collections.Generic;
using UnityEngine;
using Vices.Scripts.Core;

namespace JabberwockyWorld.Quest.Scripts
{
    public class QuestManager : MonoBehaviour
    {
        private QuestInfo _questInfo;
        private List<QuestData> _quests;

        public static QuestManager Singleton;

        private void Awake()
        {
            if (Singleton != null) 
            {
                Destroy(this);
            }

            Singleton = this;
        }

        private void Start()
        {
            _quests = new List<QuestData>();

            _questInfo = GameContext.Context.QuestInfo;
            _questInfo.SubscribeOnQuestAdd(OnQuestAdded);
        }

        public QuestData[] GetAllQuests() => _quests.ToArray();

        private void OnQuestAdded(QuestData quest)
        {
            _quests.Add(quest);
        }

        private void OnDestroy()
        {
            _quests.Clear();
            _questInfo.UnsubscribeOnQuestEnd(OnQuestAdded);
        }
    }
}