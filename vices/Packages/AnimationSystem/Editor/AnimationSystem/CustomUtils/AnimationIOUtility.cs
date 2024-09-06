using JabberwockyWorld.AnimationSystem.Editor;
using JabberwockyWorld.AnimationSystem.Editor.Data;
#if UNITY_EDITOR
using JabberwockyWorld.AnimationSystem.Editor.Elements;
#endif
using JabberwockyWorld.AnimationSystem.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor.Utilities
{
    public static class AnimationIOUtility
    {
        private static AnimationGraphView _graphView;

        private static string _graphFileName;
        private static string _containerFolderPath;
        private static string _animationSystemPath;

        private static List<AnimationNodeGraph> _nodes;
        private static List<AnimationGroup> _groups;

        private static Dictionary<string, AnimationGroupRuntimeData> _createdAnimationGroups;
        private static Dictionary<string, SimpleAnimationNode> _createdSimpleAnimations;
        private static Dictionary<string, SwitchNode> _createdSwitchAnimations;

        private static Dictionary<string, AnimationGroup> _loadedGroups;
        private static Dictionary<string, AnimationNodeGraph> _loadedNodes;

        public static void Initialize(AnimationGraphView AnimationGraphView, string graphName)
        {
            _graphView = AnimationGraphView;

            _graphFileName = graphName;
            _animationSystemPath = $"{EditorConstants.DataResources}/AnimationSystem";
            _containerFolderPath = $"{_animationSystemPath}/Animations/{graphName}";

            _nodes = new List<AnimationNodeGraph>();
            _groups = new List<AnimationGroup>();

            _createdAnimationGroups = new Dictionary<string, AnimationGroupRuntimeData>();
            _createdSimpleAnimations = new Dictionary<string, SimpleAnimationNode>();
            _createdSwitchAnimations = new Dictionary<string, SwitchNode>();

            _loadedGroups = new Dictionary<string, AnimationGroup>();
            _loadedNodes = new Dictionary<string, AnimationNodeGraph>();
        }

        public static void Save()
        {
            CreateDefaultFolders();

            GetElementsFromGraphView();

            AnimationGraphEditorData graphData = CreateAsset<AnimationGraphEditorData>($"{_animationSystemPath}/Graphs", $"{_graphFileName}Graph");

            graphData.Initialize(_graphFileName);

            AnimationCotainerRuntimeData aniamtionContainer = CreateAsset<AnimationCotainerRuntimeData>(_containerFolderPath, _graphFileName);

            aniamtionContainer.Initialize(_graphFileName);

            SaveGroups(graphData, aniamtionContainer);
            SaveNodes(graphData, aniamtionContainer);

            SaveAsset(graphData);
            SaveAsset(aniamtionContainer);
        }

        private static void SaveGroups(AnimationGraphEditorData graphData, AnimationCotainerRuntimeData aniamtionContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (AnimationGroup group in _groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, aniamtionContainer);
                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupToGraph(AnimationGroup group, AnimationGraphEditorData graphData)
        {
            AnimationGroupEditorData groupData = new AnimationGroupEditorData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };

            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToScriptableObject(AnimationGroup group, AnimationCotainerRuntimeData animationCotainer)
        {
            string groupName = group.title;

            CreateFolder($"{_containerFolderPath}/Groups", groupName);
            CreateFolder($"{_containerFolderPath}/Groups/{groupName}", "Animations");

            AnimationGroupRuntimeData animationGroup = CreateAsset<AnimationGroupRuntimeData>($"{_containerFolderPath}/Groups/{groupName}", groupName);

            animationGroup.Initialize(groupName);

            _createdAnimationGroups.Add(group.ID, animationGroup);

            animationCotainer.SimpleGroups.Add(animationGroup, new List<SimpleAnimationNode>());
            animationCotainer.SwitchGroups.Add(animationGroup, new List<SwitchNode>());

            SaveAsset(animationGroup);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, AnimationGraphEditorData graphData)
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

        private static void SaveNodes(AnimationGraphEditorData graphData, AnimationCotainerRuntimeData animationContainer)
        {
            SerializableDictionaryForAnimation<string, List<string>> groupedNodeNames = new SerializableDictionaryForAnimation<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (AnimationNodeGraph node in _nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, animationContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.Name);

                    continue;
                }

                ungroupedNodeNames.Add(node.Name);
            }

            UpdateAnimationsChoicesConnections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }

        private static void SaveNodeToGraph(AnimationNodeGraph node, AnimationGraphEditorData graphData)
        {
            List<AnimationChoiceEditorData> choices = CloneNodeChoices(node.Choices);

            AnimationGroup group = (AnimationGroup)node.Group;
            AnimationNodeEditorData nodeData = new AnimationNodeEditorData()
            {
                ID = node.ID,
                Name = node.Name,
                Choices = choices,
                Controll = node.Controll,
                GroupID = group?.ID,
                Driversofnode = node.Drivers,
                Cells = node.Cells,
                Type = node.Type,
                
                Position = node.GetPosition().position
            };

            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToScriptableObject(AnimationNodeGraph node, AnimationCotainerRuntimeData aniamtionContainer)
        {

            if (node.Type == AnimationType.SimpleAnimation)
            {
                SimpleAnimationNode animation;

                if (node.Group != null)
                {
                    animation = CreateAsset<SimpleAnimationNode>($"{_containerFolderPath}/Groups/{node.Group.title}/Animations", node.Name);
                    AnimationGroup group = (AnimationGroup)(node.Group);
                    aniamtionContainer.SimpleGroups.AddItem(_createdAnimationGroups[group.ID], animation);
                }
                else
                {
                    animation = CreateAsset<SimpleAnimationNode>($"{_containerFolderPath}/Global/Animations", node.Name);

                    aniamtionContainer.UngroupedSimpleAnimations.Add(animation);
                }

                animation.Initialize(
                    node.Name,
                    ConvertCellsToAnimationCells(node.Cells),
                    node.Controll,
                    ConvertDriversToAnimationDrivers(node.Drivers),
   
                    node.IsStartingNode()
                ) ;

                _createdSimpleAnimations.Add(node.ID, animation);

                SaveAsset(animation);

            }
            else if (node.Type == AnimationType.Switch)
            {
                SwitchNode animation;

                if (node.Group != null)
                {
                    animation = CreateAsset<SwitchNode>($"{_containerFolderPath}/Groups/{node.Group.title}/Animations", node.Name);
                    AnimationGroup group = (AnimationGroup)(node.Group);
                    aniamtionContainer.SwitchGroups.AddItem(_createdAnimationGroups[group.ID], animation);
                }
                else
                {
                    animation = CreateAsset<SwitchNode>($"{_containerFolderPath}/Global/Animations", node.Name);

                    aniamtionContainer.UngroupedSwitchs.Add(animation);
                }

                animation.Initialize(
                    node.Name,
                    ConvertNodeChoicesToAnimationChoices(node.Choices),
                    node.Controll,
                    ConvertDriversToAnimationDrivers(node.Drivers),
                    node.IsStartingNode()
                    );

                _createdSwitchAnimations.Add(node.ID, animation);

                SaveAsset(animation);
            }
        }

        private static List<AnimationCellsData> ConvertCellsToAnimationCells(List<AnimationCellEditorData> cells)
        {
            List<AnimationCellsData> animationCells = new List<AnimationCellsData>();

            foreach (AnimationCellEditorData cell in cells)
            {
                AnimationCellsData cellData = new AnimationCellsData()
                {
                    Sprite = cell.Sprite,
                    //DriverDIctDatas = ConvertDriversToAnimationDrivers(cell.DriverDIctEditorDatas)
                };

                animationCells.Add(cellData);
            }

            return animationCells;
        }

        private static List<AnimationDriverDIctData> ConvertDriversToAnimationDrivers(List<AnimationDriverDIctEditorData> drivers)
        {
            List<AnimationDriverDIctData> animationChoices = new List<AnimationDriverDIctData>();

            foreach (AnimationDriverDIctEditorData driver in drivers)
            {
                AnimationDriverDIctData driverData = new AnimationDriverDIctData()
                {
                    Controll = driver.Controll,
                    Key = driver.Key
                };

                animationChoices.Add(driverData);
            }

            return animationChoices;
        }

        private static List<AnimationChoiceRuntimeData> ConvertNodeChoicesToAnimationChoices(List<AnimationChoiceEditorData> nodeChoices)
        {
            List<AnimationChoiceRuntimeData> animationChoices = new List<AnimationChoiceRuntimeData>();

            foreach (AnimationChoiceEditorData nodeChoice in nodeChoices)
            {
                AnimationChoiceRuntimeData choiceData = new AnimationChoiceRuntimeData()
                {
                    Text = nodeChoice.Text
                };

                animationChoices.Add(choiceData);
            }

            return animationChoices;
        }

        private static void UpdateAnimationsChoicesConnections()
        {
            foreach (AnimationNodeGraph node in _nodes)
            {
                if (node.Type == AnimationType.Switch)
                {
                    SwitchNode swithc = _createdSwitchAnimations[node.ID];

                    for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
                    {
                        swithc.nodes = new AnimatorNode[node.Choices.Count];
                        AnimationChoiceEditorData nodeChoice = node.Choices[choiceIndex];

                        if (string.IsNullOrEmpty(nodeChoice.NodeID))
                        {
                            continue;
                        }

                        if (_createdSwitchAnimations.ContainsKey(nodeChoice.NodeID))
                        {
                            if (_createdSwitchAnimations[nodeChoice.NodeID] != null)
                                swithc.nodes[choiceIndex] = _createdSwitchAnimations[nodeChoice.NodeID];
                        }
                        else if (_createdSimpleAnimations.ContainsKey(nodeChoice.NodeID))
                        {
                            if (_createdSimpleAnimations[nodeChoice.NodeID] != null)
                                swithc.nodes[choiceIndex] = _createdSimpleAnimations[nodeChoice.NodeID];
                        }

                        SaveAsset(swithc);
                    }
                }
                
            }
        }

        private static void UpdateOldGroupedNodes(SerializableDictionaryForAnimation<string, List<string>> currentGroupedNodeNames, AnimationGraphEditorData graphData)
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
                        RemoveAsset($"{_containerFolderPath}/Groups/{oldGroupedNode.Key}/Animations", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeNames = new SerializableDictionaryForAnimation<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, AnimationGraphEditorData graphData)
        {
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_containerFolderPath}/Global/Animations", nodeToRemove);
                }
            }

            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        public static void Load()
        {
            AnimationGraphEditorData graphData = LoadAsset<AnimationGraphEditorData>($"{_animationSystemPath}/Graphs", _graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"{_animationSystemPath}/Graphs/{_graphFileName}\".\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }

            AnimationEditorWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }

        private static void LoadGroups(List<AnimationGroupEditorData> groups)
        {
            foreach (AnimationGroupEditorData groupData in groups)
            {
                AnimationGroup group = (AnimationGroup)_graphView.CreateGroup(groupData.Name, groupData.Position);

                group.ID = groupData.ID;

                _loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<AnimationNodeEditorData> nodes)
        {
            foreach (AnimationNodeEditorData nodeData in nodes)
            {
                List<AnimationChoiceEditorData> choices = CloneNodeChoices(nodeData.Choices);

                AnimationNodeGraph node = _graphView.CreateNode(nodeData.Name, nodeData.Type, nodeData.Position, false);

                node.ID = nodeData.ID;
                node.Controll = nodeData.Controll;
                node.Drivers = nodeData.Driversofnode;
                if (node.Type == AnimationType.Switch)
                {
                    node.Choices = choices;
                }
                else if (node.Type == AnimationType.SimpleAnimation)
                {
                    node.Cells = nodeData.Cells;
                }

                node.Draw();

                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                AnimationGroup group = _loadedGroups[nodeData.GroupID];

                node.Group = group;

                group.AddElement(node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, AnimationNodeGraph> loadedNode in _loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    AnimationChoiceEditorData choiceData = (AnimationChoiceEditorData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        continue;
                    }
                    
                    if (_loadedNodes.ContainsKey(choiceData.NodeID))
                    {
                        AnimationNodeGraph nextNode = _loadedNodes[choiceData.NodeID];
                        Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();
                        Edge edge = choicePort.ConnectTo(nextNodeInputPort);
                        _graphView.AddElement(edge);
                    }
                    

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        private static void LoadDrivers()
        {
            foreach(KeyValuePair<string, AnimationNodeGraph> loadedNode in _loadedNodes)
            {
                
            }
        }

        private static void CreateDefaultFolders()
        {
            CreateFolder(EditorConstants.DataResources, "AniamtionSystem");
            CreateFolder(_animationSystemPath, "Graphs");
            CreateFolder(_animationSystemPath, "Animations");
            CreateFolder($"{_animationSystemPath}/Animations", _graphFileName);

            CreateFolder(_containerFolderPath, "Global");
            CreateFolder(_containerFolderPath, "Groups");
            CreateFolder($"{_containerFolderPath}/Global", "Animations");
        }

        private static void GetElementsFromGraphView()
        {
            Type groupType = typeof(AnimationGroup);

            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is AnimationNodeGraph node)
                {
                    _nodes.Add(node);

                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    AnimationGroup group = (AnimationGroup)graphElement;

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
            string fullPath = $"{path}/{assetName}.asset";

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

        private static List<AnimationChoiceEditorData> CloneNodeChoices(List<AnimationChoiceEditorData> nodeChoices)
        {
            List<AnimationChoiceEditorData> choices = new List<AnimationChoiceEditorData>();

            foreach (AnimationChoiceEditorData choice in nodeChoices)
            {
                AnimationChoiceEditorData choiceData = new AnimationChoiceEditorData()
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
#endif