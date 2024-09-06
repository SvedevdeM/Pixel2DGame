using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Scripts.Data
{
    [CreateAssetMenu(fileName = nameof(ActorInfo), menuName = "JabberwockyWorld/Dialogue/" + nameof(ActorInfo))]
    public class ActorInfo : ScriptableObject
    {
        public Sprite Icon;
        public string Name;
        public bool IsPlayer;
    }
}