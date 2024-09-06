using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Scripts.Structs;
using UnityEngine;

namespace JabberwockyWorld.Quest.Scripts
{
    [CreateAssetMenu(fileName = nameof(QuestData), menuName = "JabberwockyWorld/Quest/" + nameof(QuestData))]
    public class QuestData : ScriptableObject
    {
        public Sprite Image;
        public string Name;
        public string MainPoint;
        [TextArea()]
        public string Description;
        public QuestType Type;

        [Header("Complete Quest On")]
        public EventField OnQuestComplete;
    }
}
