using UnityEngine;


namespace JabberwockyWorld.AnimationSystem.Scripts
{
    public class AnimationNode<TCel> : TermNode
        where TCel : ICEL
    {
        public static TNode Create<TNode>(
            ControlDriverNew driver = null,
            TCel[] cels = null
        ) where TNode : AnimationNode<TCel>
        {
            var instance = CreateInstance<TNode>();

            if (driver != null)
                instance.controlDriverNew = driver;
            if (cels != null)
                instance.cels = cels;

            return instance;
        }

        [SerializeField] public string Name;
        [SerializeField] protected TCel[] cels;
        [SerializeField] protected ControlDriverNew controlDriverNew = new ControlDriverNew(true);
        [SerializeField] protected DriverDictionary drivers = new DriverDictionary();
        [SerializeField] public bool IsStartingNode;
        [SerializeField] public const AnimationType AnimationTypeCo = AnimationType.SimpleAnimation;

        public override TermNode Resolve(IReadOnlyAnimatorState previousState, AnimatorState nextState)
        {
            nextState.Merge(drivers);
            return this;
        }

        public override ICEL ResolveCel(IReadOnlyAnimatorState previousState, AnimatorState nextState)
        {
            nextState.Merge(drivers);
            //Debug.Log("smthwrong");
            return cels[controlDriverNew.ResolveDriver(previousState, nextState, cels.Length)];
        }
    }
}