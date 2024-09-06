using System;
using UnityEngine;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Data;


namespace JabberwockyWorld.DynamicEvents.Scripts.Structs
{
    [Serializable]
    public struct RuleField
    {
        public RuleEntry RuleEntry;
        private RuleEntry _ruleEntry;

        private EventDatabase _database;

        public void Initialize()
        {
            _database = Resources.Load<EventDatabase>("Data/EventDatabase/EventDatabase");
            if (!_database.TryFindRule(RuleEntry.ID, out _ruleEntry)) Debug.LogError($"Can't find {RuleEntry.Name} of {typeof(RuleEntry)}");
        }

        public void Execute()
        {
            if (_ruleEntry == null)
            {
                Debug.LogWarning($"Trying to change {RuleEntry.Name} when it isn't setted. Please set rule, {this}");
                return;
            }

            _ruleEntry.Execute();
        }
    }
}
