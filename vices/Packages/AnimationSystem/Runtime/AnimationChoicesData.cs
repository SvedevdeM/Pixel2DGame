using System;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [Serializable]
    public class AnimationChoiceRuntimeData
    {
        public string Text;
        public AnimatorNode NextAnimation;
    }
}
