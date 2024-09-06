using System;


namespace JabberwockyWorld.DynamicEvents.Scripts.Entries
{
    [Serializable]
    public class FactEntry : BaseEntry
    {
        public int Value;

        public FactEntry(string name, string Id) : base(name, Id)
        {
        }
    }
}
