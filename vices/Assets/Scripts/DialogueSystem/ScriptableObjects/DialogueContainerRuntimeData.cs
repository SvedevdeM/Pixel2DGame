using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Scripts.Data
{
    public class DialogueContainerRuntimeData : ScriptableObject
    {
        [field: SerializeField] public string FileName;
        [field: SerializeField] public SerializableDictionary<DialogueGroupRuntimeData, List<DialogueRuntimeData>> Groups;
        [field: SerializeField] public List<DialogueRuntimeData> UngroupedDialogues;

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new SerializableDictionary<DialogueGroupRuntimeData, List<DialogueRuntimeData>>();
            UngroupedDialogues = new List<DialogueRuntimeData>();
        }
    }
}
