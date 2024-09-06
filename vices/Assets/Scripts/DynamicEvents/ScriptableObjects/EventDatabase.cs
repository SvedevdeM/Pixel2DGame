using System.Collections.Generic;
using UnityEngine;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using Vices.Scripts.Core;


namespace JabberwockyWorld.DynamicEvents.Scripts.Data
{
    [CreateAssetMenu(fileName = "EventDatabase", menuName = "JabberwockyWorld/DynamicEvents/EventDatabase")]
    public class EventDatabase : ScriptableObject, IExecutable
    {
        public List<EventTable> EventTables;
        public List<RuleEntry> RuleEntries;

        public void AddActiveRule(RuleEntry ruleEntry)
        {
            RuleEntries.Add(ruleEntry);
        }

        public void Execute()
        {
            if (RuleEntries.Count <= 0) return;

            for (int i = 0; i < RuleEntries.Count; i++)
            {
                RuleEntries[i].Execute();
            }
        }

        public void RemoveActiveRule(RuleEntry rule)
        {
            RuleEntries.Remove(rule);
        }

        public void InitializeEntries()
        {
            for (int i = 0; i < EventTables.Count; i++)
            {
                var eventTable = EventTables[i];

                for (int j = 0; j < eventTable.Events.Count; j++)
                {
                    eventTable.Events[j].Initialize(this);
                    eventTable.Events[j].Usages = 0;
                }

                for (int j = 0; j < eventTable.Rules.Count; j++)
                {
                    eventTable.Rules[j].Initialize(this);
                    eventTable.Rules[j].Usages = 0;
                }

                //TODO: Think about what actually facts need have 
                for (int j = 0; j < eventTable.Facts.Count; j++)
                {
                    eventTable.Facts[j].Initialize(this);
                    eventTable.Facts[j].Value = 0;
                }
            }
        }

        #region TryFind Methods
        public bool TryFindEvent(string id, out EventEntry eventEntry)
        {
            eventEntry = default;
            for (int i = 0; i < EventTables.Count; i++)
            {
                for (int j = 0; j < EventTables[i].Events.Count; j++)
                {
                    if (EventTables[i].Events[j].ID == id)
                    {
                        eventEntry = EventTables[i].Events[j];
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryFindFact(string id, out FactEntry factEntry)
        {
            factEntry = default;
            for (int i = 0; i < EventTables.Count; i++)
            {
                for (int j = 0; j < EventTables[i].Facts.Count; j++)
                {
                    if (EventTables[i].Facts[j].ID == id)
                    {
                        factEntry = EventTables[i].Facts[j];
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryFindRule(string id, out RuleEntry ruleEntry)
        {
            ruleEntry = default;
            for (int i = 0; i < EventTables.Count; i++)
            {
                for (int j = 0; j < EventTables[i].Rules.Count; j++)
                {
                    if (EventTables[i].Rules[j].ID == id)
                    {
                        ruleEntry = EventTables[i].Rules[j];
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
    }
}