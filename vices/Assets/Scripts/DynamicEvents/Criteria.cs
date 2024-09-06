using System;
using JabberwockyWorld.DynamicEvents.Scripts.Entries;


namespace JabberwockyWorld.DynamicEvents.Scripts
{
    [Serializable]
    public partial class Criteria
    {
        public FactEntry Fact;
        public EqualType Type;
        public int Value;

        public Criteria(string name, string id)
        {
        }

        public bool CheckCriteria()
        {
            switch (Type)
            {
                case EqualType.None:
                    return false;
                case EqualType.Greater:
                    if (Fact.Value > Value) return true;
                    break;
                case EqualType.GreaterOrEqual:
                    if (Fact.Value >= Value) return true;
                    break;
                case EqualType.Less:
                    if (Fact.Value < Value) return true;
                    break;
                case EqualType.LessOrEqual:
                    if (Fact.Value <= Value) return true;
                    break;
                case EqualType.Equal:
                    if (Fact.Value == Value) return true;
                    break;
                default:
                    return false;
            }

            return false;
        }
    }
}
