using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [CreateAssetMenu(fileName = "switch", menuName = "Animation System/Switch Node", order = 400)]
    public class SwitchNode : AnimatorNode
    {
        [SerializeField] public string Name;
        [SerializeField] public AnimatorNode[] nodes;
        [Tooltip("SpriteRenderer being used to display the animation.")]
        [SerializeField] protected ControlDriverNew controlDriver = new ControlDriverNew(false);
        [SerializeField] protected DriverDictionary drivers = new DriverDictionary();
        [SerializeField] public bool IsStartingNode;
        [SerializeField] public const AnimationType AnimationTypeCo = AnimationType.Switch;

        public override TermNode Resolve(IReadOnlyAnimatorState previousState, AnimatorState nextState)
        {
            nextState.Merge(drivers);
            return nodes[controlDriver.ResolveDriver(previousState, nextState, nodes.Length)]
                .Resolve(previousState, nextState);
        }

        public void Initialize(string name, List<AnimationChoiceRuntimeData> choises, Controll controll, List<AnimationDriverDIctData> newdrivers, bool isStartingNode)
        {
            Name = name;
            controlDriver.ChangeControl(controll);
            IsStartingNode = isStartingNode;

            foreach (AnimationDriverDIctData driverD in newdrivers)
            {
                if (driverD.Controll != null)
                {
                    drivers.values.Add(driverD.Key);
                    drivers.keys.Add(driverD.Controll);
                }
            }

        }
    }
}