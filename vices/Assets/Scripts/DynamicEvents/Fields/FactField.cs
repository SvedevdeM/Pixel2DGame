using System;
using UnityEngine;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Data;


namespace JabberwockyWorld.DynamicEvents.Scripts.Structs
{
    [Serializable]
    public struct FactField
    {
        public FactEntry FactEntry;

        private EventDatabase _database;

        public void Initialize()
        {
            _database = Resources.Load<EventDatabase>("Data/EventDatabase/EventDatabase");

            if (!_database.TryFindFact(FactEntry.ID, out FactEntry)) Debug.LogError($"Can't find {FactEntry.Name} of {typeof(FactEntry)}");
        }

        public void ChangeFact()
        {
            if (FactEntry == null) 
            {
                Debug.LogWarning($"Trying to change {FactEntry.Name} when it isn't setted. Please set fact, {this}");
                return; 
            }

            FactEntry.Value++;
        }
    }
}
