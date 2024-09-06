using System;


namespace JabberwockyWorld.DynamicEvents.Scripts
{
    public partial class Criteria
    {
        [Serializable]
        public enum EqualType
        {
            None           = 0,
            Greater        = 1,
            GreaterOrEqual = 2,
            Less           = 3,
            LessOrEqual    = 4,
            Equal          = 5
        }
    }
}
