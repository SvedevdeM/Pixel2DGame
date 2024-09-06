#if UNITY_EDITOR
using JabberwockyWorld.AnimationSystem.Editor.Data;
using JabberwockyWorld.AnimationSystem.Editor.Elements;
using System.Collections.Generic;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    public class AnimationNodeErrorData
    {
        public AnimationErrorData Error { get; set; }
        public List<AnimationNodeGraph> Nodes { get; set; }

        public AnimationNodeErrorData()
        {
            Error = new AnimationErrorData();
            Nodes = new List<AnimationNodeGraph>();
        }
    }
}
#endif