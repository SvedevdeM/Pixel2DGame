using System;
using UnityEngine;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Data;


namespace JabberwockyWorld.DynamicEvents.Scripts.Structs
{
    [Serializable]
    public struct EventField
    {
        public EventEntry EventEntry;
        private EventEntry _eventEntry;

        private EventDatabase _database;

        public void Initialize()
        {
            _database = Resources.Load<EventDatabase>("Data/EventDatabase/EventDatabase");

            if (!_database.TryFindEvent(EventEntry.ID, out _eventEntry)) Debug.LogError($"Can't find {EventEntry.Name} of {typeof(EventEntry)}");
        }

        public void SubscribeOnEvent(Action action = null)
        {
            if (_eventEntry == null) return;
            _eventEntry.OnEventTriggered += action;
        }

        public void TriggerEvent()
        {
            if (_eventEntry == null) return;
            if (!_eventEntry.CheckEntires()) return;

            _eventEntry.Execute();
            _eventEntry.OnEventTriggered?.Invoke();
        }
    }
}