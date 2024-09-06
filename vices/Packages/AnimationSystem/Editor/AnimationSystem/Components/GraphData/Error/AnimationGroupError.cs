#if UNITY_EDITOR
using System.Collections.Generic;
using JabberwockyWorld.AnimationSystem.Editor;
using JabberwockyWorld.AnimationSystem.Editor.Elements;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    public class AnimationGroupErrorData
    {
        public AnimationErrorData Error { get; set; }
        public List<AnimationGroup> Groups { get; set; }

        public AnimationGroupErrorData()
        {
            Error = new AnimationErrorData();
            Groups = new List<AnimationGroup>();
        }
    }
}
#endif