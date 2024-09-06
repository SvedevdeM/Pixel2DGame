using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    public class AnimationCotainerRuntimeData : ScriptableObject
    {
        [field: SerializeField] public string FileName;
        [field: SerializeField] public SerializableDictionaryForAnimation<AnimationGroupRuntimeData, List<SimpleAnimationNode>> SimpleGroups;
        [field: SerializeField] public SerializableDictionaryForAnimation<AnimationGroupRuntimeData, List<SwitchNode>> SwitchGroups;
        [field: SerializeField] public List<SimpleAnimationNode> UngroupedSimpleAnimations;
        [field: SerializeField] public List<SwitchNode> UngroupedSwitchs;

        public void Initialize(string fileName)
        {
            FileName = fileName;
            SimpleGroups = new SerializableDictionaryForAnimation<AnimationGroupRuntimeData, List<SimpleAnimationNode>>();
            UngroupedSimpleAnimations = new List<SimpleAnimationNode>();
            UngroupedSwitchs = new List<SwitchNode>();
        }
    }
}
