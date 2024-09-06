using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    public abstract class TermNode : AnimatorNode
    {
        public abstract ICEL ResolveCel(IReadOnlyAnimatorState previousState, AnimatorState nextState);
    }
}