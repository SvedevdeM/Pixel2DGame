using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor.Data
{
    public class DialogueGraphEditorData : ScriptableObject
    {
        [SerializeField] public string FileName;
        [SerializeField] public List<DialogueGroupEditorData> Groups;
        [SerializeField] public List<DialogueNodeEditorData> Nodes;
        [SerializeField] public List<string> OldGroupNames;
        [SerializeField] public List<string> OldUngroupedNodeNames;

        [SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames;

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<DialogueGroupEditorData>();
            Nodes = new List<DialogueNodeEditorData>();

            OldGroupNames = new List<string>();
            OldUngroupedNodeNames = new List<string>();

            OldGroupedNodeNames = new SerializableDictionary<string, List<string>>();
        }
    }

}