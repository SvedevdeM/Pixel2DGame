using System;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [Serializable]
    public struct OverridePair
    {
        public TermNode fromNode;
        public TermNode toNode;
    }

    [CreateAssetMenu(fileName = "override", menuName = "Animation System/Override Node", order = 400)]
    public class OverrideNode : AnimatorNode
    {
        [SerializeField] private AnimatorNode next;
        [SerializeField] private List<OverridePair> overrides = new List<OverridePair>();

        private readonly Dictionary<TermNode, TermNode> _map =
            new Dictionary<TermNode, TermNode>();

        private void OnEnable()
        {
            _map.Clear();
            foreach (var pair in overrides)
            {
                if (pair.fromNode == null || pair.toNode == null) continue;
                _map[pair.fromNode] = pair.toNode;
            }
        }

        public override TermNode Resolve(IReadOnlyAnimatorState previousState, AnimatorState nextState)
        {
            var node = next.Resolve(previousState, nextState);
            if (!_map.ContainsKey(node))
                return node;

            var overrideNode = _map[node];
            return overrideNode;
        }
    }
}