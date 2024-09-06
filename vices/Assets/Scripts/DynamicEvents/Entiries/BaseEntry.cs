using JabberwockyWorld.DynamicEvents.Scripts.Data;
using System;
using UnityEngine;


namespace JabberwockyWorld.DynamicEvents.Scripts.Entries 
{
    [Serializable]
    public class BaseEntry
    {
        [SerializeField] public string Name;
        [SerializeField] public string ID;
        [SerializeField] public float Usages;
        [SerializeField] public bool Once;

        protected EventDatabase _database;
        protected virtual void OnInitialize() { }

        public BaseEntry(string name, string Id)
        {
            Name = name;
            ID = Id;
        }

        public void Initialize(EventDatabase database)
        {
            _database = database;
            OnInitialize();
        }
    }
}