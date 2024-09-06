using JabberwockyWorld.DialogueSystem.Editor.Data;
using JabberwockyWorld.DialogueSystem.Editor.Elements;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.Editor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor.Utilities
{
    public static class DialogueIOUtility
    {
        private static DialogueGraphView _graphView;

        private static string _graphFileName;
        private static string _containerFolderPath;
        private static string _dialogueSystemPath;

        private static List<GraphNode> _nodes;
        private static List<GraphGroup> _groups;

        private static Dictionary<string, DialogueGroupRuntimeData> _createdDialogueGroups;
        private static Dictionary<string, DialogueRuntimeData> _createdDialogues;

        private static Dictionary<string, GraphGroup> _loadedGroups;
        private static Dictionary<string, GraphNode> _loadedNodes;

        public static void Initialize(DialogueGraphView DialogueGraphView, string graphName)
        {
            _graphView = DialogueGraphView;

            _graphFileName = graphName;
            _dialogueSystemPath = $"{EditorConstants.DataResources}/DialogueSystem";
            _containerFolderPath = $"{_dialogueSystemPath}/Dialogues/{graphName}";

            _nodes = new List<GraphNode>();
            _groups = new List<GraphGroup>();

            _createdDialogueGroups = new Dictionary<string, DialogueGroupRuntimeData>();
            _createdDialogues = new Dictionary<string, DialogueRuntimeData>();

            _loadedGroups = new Dictionary<string, GraphGroup>();
            _loadedNodes = new Dictionary<string, GraphNode>();
        }

        public static void Save()
        {
            CreateDefaultFolders();

            GetElementsFromGraphView();

            DialogueGraphEditorData graphData = CreateAsset<DialogueGraphEditorData>($"{_dialogueSystemPath}/Graphs", $"{_graphFileName}Graph");

            graphData.Initialize(_graphFileName);

            DialogueContainerRuntimeData dialogueContainer = CreateAsset<DialogueContainerRuntimeData>(_containerFolderPath, _graphFileName);

            dialogueContainer.Initialize(_graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        private static void SaveGroups(DialogueGraphEditorData graphData, DialogueContainerRuntimeData dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (GraphGroup group in _groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, dialogueContainer);
                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupToGraph(GraphGroup group, DialogueGraphEditorData graphData)
        {
            DialogueGroupEditorData groupData = new DialogueGroupEditorData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };

            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToScriptableObject(GraphGroup group, DialogueContainerRuntimeData dialogueContainer)
        {
            string groupName = group.title;

            CreateFolder($"{_containerFolderPath}/Groups", groupName);
            CreateFolder($"{_containerFolderPath}/Groups/{groupName}", "Dialogues");

            DialogueGroupRuntimeData dialogueGroup = CreateAsset<DialogueGroupRuntimeData>($"{_containerFolderPath}/Groups/{groupName}", groupName);

            dialogueGroup.Initialize(groupName);

            _createdDialogueGroups.Add(group.ID, dialogueGroup);

            dialogueContainer.Groups.Add(dialogueGroup, new List<DialogueRuntimeData>());

            SaveAsset(dialogueGroup);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, DialogueGraphEditorData graphData)
        {
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{_containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        private static void SaveNodes(DialogueGraphEditorData graphData, DialogueContainerRuntimeData dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (GraphNode node in _nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.Name);

                    continue;
                }

                ungroupedNodeNames.Add(node.Name);
            }

            UpdateDialoguesChoicesConnections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }

        private static void SaveNodeToGraph(GraphNode node, DialogueGraphEditorData graphData)
        {
            List<DialogueChoiceEditorData> choices = CloneNodeChoices(node.Choices);

            GraphGroup group = (GraphGroup)node.Group;
            DialogueNodeEditorData nodeData = new DialogueNodeEditorData()
            {
                ID = node.ID,
                Name = node.Name,
                Choices = choices,
                Text = node.Text,
                GroupID = group?.ID,
                Actor = (ActorInfo)node.Actor,
                Event = node.Event,
                Type = node.Type,
                Position = node.GetPosition().position
            };

            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToScriptableObject(GraphNode node, DialogueContainerRuntimeData dialogueContainer)
        {
            DialogueRuntimeData dialogue;

            if (node.Group != null)
            {
                dialogue = CreateAsset<DialogueRuntimeData>($"{_containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.Name);

                GraphGroup group = (GraphGroup)node.Group;

                dialogueContainer.Groups.AddItem(_createdDialogueGroups[group.ID], dialogue);
            }
            else
            {
                dialogue = CreateAsset<DialogueRuntimeData>($"{_containerFolderPath}/Global/Dialogues", node.Name);

                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }

            dialogue.Initialize(
                node.Name,
                node.Text,
                ConvertNodeChoicesToDialogueChoices(node.Choices),
                node.Type,
                node.IsStartingNode,
                node.Actor,
                node.Event
            );

            _createdDialogues.Add(node.ID, dialogue);

            SaveAsset(dialogue);
        }

        private static List<DialogueChoiceRuntimeData> ConvertNodeChoicesToDialogueChoices(List<DialogueChoiceEditorData> nodeChoices)
        {
            List<DialogueChoiceRuntimeData> dialogueChoices = new List<DialogueChoiceRuntimeData>();

            foreach (DialogueChoiceEditorData nodeChoice in nodeChoices)
            {
                DialogueChoiceRuntimeData choiceData = new DialogueChoiceRuntimeData()
                {
                    Text = nodeChoice.Text
                };

                dialogueChoices.Add(choiceData);
            }

            return dialogueChoices;
        }

        private static void UpdateDialoguesChoicesConnections()
        {
            foreach (GraphNode node in _nodes)
            {
                DialogueRuntimeData dialogue = _createdDialogues[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
                {
                    DialogueChoiceEditorData nodeChoice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(nodeChoice.NodeID))
                    {
                        continue;
                    }

                    dialogue.Choices[choiceIndex].NextDialogue = _createdDialogues[nodeChoice.NodeID];

                    SaveAsset(dialogue);
                }
            }
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DialogueGraphEditorData graphData)
        {
            if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{_containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DialogueGraphEditorData graphData)
        {
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }

            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        public static void Load()
        {
            DialogueGraphEditorData graphData = LoadAsset<DialogueGraphEditorData>($"{_dialogueSystemPath}/Graphs", _graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"{_dialogueSystemPath}/Graphs/{_graphFileName}\".\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }

            DialogueEditorWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }

        private static void LoadGroups(List<DialogueGroupEditorData> groups)
        {
            foreach (DialogueGroupEditorData groupData in groups)
            {
                GraphGroup group = (GraphGroup)_graphView.CreateGroup(groupData.Name, groupData.Position);

                group.ID = groupData.ID;

                _loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<DialogueNodeEditorData> nodes)
        {
            foreach (DialogueNodeEditorData nodeData in nodes)
            {
                List<DialogueChoiceEditorData> choices = CloneNodeChoices(nodeData.Choices);

                GraphNode node = _graphView.CreateNode(nodeData.Name, nodeData.Type, nodeData.Position, false);

                node.ID = nodeData.ID;
                node.Choices = choices;
                node.Text = nodeData.Text;
                node.Actor = nodeData.Actor;
                node.Event = nodeData.Event;

                node.Draw();

                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                GraphGroup group = _loadedGroups[nodeData.GroupID];

                node.Group = group;

                group.AddElement(node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, GraphNode> loadedNode in _loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    DialogueChoiceEditorData choiceData = (DialogueChoiceEditorData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        continue;
                    }

                    GraphNode nextNode = _loadedNodes[choiceData.NodeID];

                    Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();

                    Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                    _graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        private static void CreateDefaultFolders()
        {
            CreateFolder(EditorConstants.DataResources, "DialogueSystem");
            CreateFolder(_dialogueSystemPath, "Graphs");
            CreateFolder(_dialogueSystemPath, "Dialogues");
            CreateFolder($"{_dialogueSystemPath}/Dialogues", _graphFileName);

            CreateFolder(_containerFolderPath, "Global");
            CreateFolder(_containerFolderPath, "Groups");
            CreateFolder($"{_containerFolderPath}/Global", "Dialogues");
        }

        private static void GetElementsFromGraphView()
        {
            Type groupType = typeof(GraphGroup);

            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is GraphNode node)
                {
                    _nodes.Add(node);

                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    GraphGroup group = (GraphGroup)graphElement;

                    _groups.Add(group);

                    return;
                }
            });
        }

        public static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        public static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static List<DialogueChoiceEditorData> CloneNodeChoices(List<DialogueChoiceEditorData> nodeChoices)
        {
            List<DialogueChoiceEditorData> choices = new List<DialogueChoiceEditorData>();

            foreach (DialogueChoiceEditorData choice in nodeChoices)
            {
                DialogueChoiceEditorData choiceData = new DialogueChoiceEditorData()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                };

                choices.Add(choiceData);
            }

            return choices;
        }
    }
}