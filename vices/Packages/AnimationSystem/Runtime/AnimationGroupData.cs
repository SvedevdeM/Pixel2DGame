using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{

    public class AnimationGroupRuntimeData : ScriptableObject
    {
        [field: SerializeField] public string Name;

        public void Initialize(string name)
        {
            Name = name;
        }
    }
}
