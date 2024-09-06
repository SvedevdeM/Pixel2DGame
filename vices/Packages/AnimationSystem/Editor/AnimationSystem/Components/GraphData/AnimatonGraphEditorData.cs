using JabberwockyWorld.AnimationSystem.Editor.Data;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    public class AnimationGraphEditorData : ScriptableObject
    {
        [SerializeField] public string FileName;
        [SerializeField] public List<AnimationGroupEditorData> Groups;
        [SerializeField] public List<AnimationNodeEditorData> Nodes;
        [SerializeField] public List<SwitchNodeEditorData> SwitchNodes;
        [SerializeField] public List<string> OldGroupNames;
        [SerializeField] public List<string> OldUngroupedNodeNames;
        [SerializeField] public List<string> OldUngroupedSwitchNames;

        [SerializeField] public SerializableDictionaryForAnimation<string, List<string>> OldGroupedNodeNames;
        [SerializeField] public SerializableDictionaryForAnimation<string, List<string>> OldGroupedSwitchNames;

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<AnimationGroupEditorData>();
            Nodes = new List<AnimationNodeEditorData>();
            SwitchNodes = new List<SwitchNodeEditorData>();

            OldGroupNames = new List<string>();
            OldUngroupedNodeNames = new List<string>();
            OldUngroupedSwitchNames = new List<string>();

            OldGroupedNodeNames = new SerializableDictionaryForAnimation<string, List<string>>();
            OldGroupedSwitchNames = new SerializableDictionaryForAnimation<string, List<string>>();
        }
    }

}