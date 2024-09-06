using System;
using JabberwockyWorld.DynamicEvents.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Scripts.Entries;


namespace JabberwockyWorld.DynamicEvents.Scripts
{
    [Serializable]
    public class Modifier
    {
        public FactEntry Fact;
        public ModifierType Type;
        public int Value;

        private EventDatabase _database;

        public Modifier(string value, string i)
        {
        }

        public void Initialize(EventDatabase database)
        {
            _database = database;
        }

        public void Modify()
        {
            //TODO: Write error message
            if (!_database.TryFindFact(Fact.ID, out var fact)) return;

            switch (Type)
            {
                case ModifierType.None:
                    break;
                case ModifierType.Plus:
                    fact.Value += Value;
                    break;
                case ModifierType.Minus:
                    fact.Value -= Value;
                    break;
                default:
                    break;
            }
        }

        public enum ModifierType
        {
            None = 0,
            Plus = 1,
            Minus = 2
        }
    }
}