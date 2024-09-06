using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Scripts.Data
{
    public class DialogueGroupRuntimeData : ScriptableObject
    {
        [field: SerializeField] public string Name;

        public void Initialize(string name)
        {
            Name = name;
        }
    }
}
