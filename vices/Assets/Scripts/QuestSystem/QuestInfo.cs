using System;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.Quest.Scripts
{
    [CreateAssetMenu(fileName = nameof(QuestInfo), menuName = "JabberwockyWorld/Quest/" + nameof(QuestInfo))]
    public class QuestInfo : ScriptableObject
    {
        private List<QuestData> _questsData;
        public List<QuestData> Quests => _questsData;

        private Action<QuestData> _onQuestAdded;
        private Action<QuestData> _onQuestEnded;

        public void AddQuest(QuestData quest)
        {
            _questsData.Add(quest);
            _onQuestAdded?.Invoke(quest);
        }

        public void EndQuest(QuestData quest)
        {
            _onQuestEnded?.Invoke(quest);
        }

        public void SubscribeOnQuestAdd(Action<QuestData> onAdd)
        {
            _onQuestAdded += onAdd;
        }

        public void SubscribeOnQuestEnd(Action<QuestData> onAdd)
        {
            _onQuestEnded += onAdd;
        }
        public void UnsubscribeOnQuestAdd(Action<QuestData> onEnd)
        {
            _onQuestAdded -= onEnd;
        }

        public void UnsubscribeOnQuestEnd(Action<QuestData> onEnd)
        {
            _onQuestEnded -= onEnd;
        }        
    }
}
