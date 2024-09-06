using System.Collections.Generic;
using System;
using UnityEngine;


namespace JabberwockyWorld.DynamicEvents.Scripts.Entries
{
    [Serializable]
    public class RuleEntry : LogicalEntry
    {
        public EventEntry TriggeredBy;
        public List<EventEntry> Triggers = new List<EventEntry>();

        public RuleEntry(string name, string Id) : base(name, Id)
        {
        }

        public override void Execute()
        {
            if (Triggers.Count <= 0) 
            {
                _database.TryFindRule(ID, out var rule);
                _database.RemoveActiveRule(rule);

                UnityEngine.Debug.LogWarning($"Remove rule {Name}, {ID}, because it have 0 triggers. \n Please set triggers or don't use rule");
            }

            for (int i = 0; i < Triggers.Count; i++)
            {
                Triggers[i].Initialize(_database);
            }

            for (int i = 0; i < Triggers.Count; i++)
            {
                _database.TryFindEvent(Triggers[i].ID, out var trigger);
                if (trigger.CheckEntires())
                {                         
                    _database.TryFindRule(ID, out var rule);
                    _database.RemoveActiveRule(rule);

                    if (trigger.CheckEntires()) trigger.Execute();

                    break;
                }
            }

            base.Execute();
        }
    }
}
