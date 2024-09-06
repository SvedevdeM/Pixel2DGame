using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Scripts.Enumerators;
using JabberwockyWorld.Quest.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace JabberwockyWorld.DynamicEvents.Scripts.Entries
{
    [Serializable]
    public class EventEntry : LogicalEntry
    {
        public RuleEntry Rule = null;
        public Action OnEventTriggered;
        public EventEntryType Type;

        public UnityEngine.Object Object;

        public EventEntry(string name, string Id) : base(name, Id)
        {
        }

        public override void Execute()
        {
            switch (Type)
            {
                case EventEntryType.None:
                    break;
                case EventEntryType.Dialogue:
                    //TODO: Change path to constant path
                    DialogueContainerInfo dialogueInfo = Resources.Load<DialogueContainerInfo>("Data/DialogueSystem/DialogueContainerInfo");
                    dialogueInfo.ChangeDialogueContainer((DialogueContainerRuntimeData)Object);
                    break;
                case EventEntryType.Quest:
                    QuestInfo questInfo = Resources.Load<QuestInfo>("Data/QuestSystem/QuestInfo");
                    questInfo.AddQuest((QuestData)Object);
                    break;
                case EventEntryType.RandomDialogue:
                    DialogueContainerInfo info = Resources.Load<DialogueContainerInfo>("Data/DialogueSystem/DialogueContainerInfo");
                    var randomDialogues = (DialogueRandomData)Object;
                    info.ChangeDialogueContainer(randomDialogues.Dialogues[UnityEngine.Random.Range(0, randomDialogues.Dialogues.Count)]);
                    break;
                default:
                    break;
            }
            base.Execute();
            if (Rule.ID == null) return;

            if (_database.TryFindRule(Rule.ID, out var rule))
                _database.AddActiveRule(rule);
        }
    }
}