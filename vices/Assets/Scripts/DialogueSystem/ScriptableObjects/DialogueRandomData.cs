using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Scripts.Data
{
    [CreateAssetMenu(fileName = nameof(DialogueRandomData), menuName = "JabberwockyWorld/Dialogue/" + nameof(DialogueRandomData))]

    public class DialogueRandomData : ScriptableObject
    {
        public List<DialogueContainerRuntimeData> Dialogues;
    }
}
