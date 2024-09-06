using System;
using System.Collections.Generic;


namespace JabberwockyWorld.DynamicEvents.Scripts.Entries
{
    [Serializable]
    public class LogicalEntry : BaseEntry
    {
        public List<Criteria> Criterias = new List<Criteria>();
        public List<Modifier> Modifications = new List<Modifier>();

        public LogicalEntry(string name, string Id) : base(name, Id)
        {
        }

        protected override void OnInitialize()
        {
            for (int i = 0; i < Modifications.Count; i++)
            {
                Modifications[i].Initialize(_database);
            }
        }

        public virtual void Execute()
        {
            if (Modifications.Count > 0)
            {
                //Refresh chosen facts
                for (int i = 0; i < Modifications.Count; i++)
                {
                    _database.TryFindFact(Modifications[i].Fact.ID, out Modifications[i].Fact);
                }

                for (int i = 0; i < Modifications.Count; i++)
                {
                    Modifications[i].Modify();
                }
            }

            Usages++;
        }

        public bool CheckEntires()
        {
            if (Once && Usages >= 1) return false;

            if (Criterias.Count <= 0) return true;

            //Refresh chosen facts
            for (int i = 0; i < Criterias.Count; i++)
            {
                _database.TryFindFact(Criterias[i].Fact.ID, out Criterias[i].Fact);
            }

            for (int i = 0; i < Criterias.Count; i++)
            {
                if (!Criterias[i].CheckCriteria()) return false;
            }

            return true;
        }
    }
}
