using UnityEngine;
using JabberwockyWorld.AnimationSystem.Scripts;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    public abstract class AnimatorNode : ScriptableObject
    {
        public abstract TermNode Resolve(IReadOnlyAnimatorState previousState, AnimatorState nextState);
    }

}