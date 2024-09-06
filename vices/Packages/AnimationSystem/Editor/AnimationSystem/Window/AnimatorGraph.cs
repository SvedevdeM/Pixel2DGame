#if UNITY_EDITOR
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using JabberwockyWorld.AnimationSystem.Editor.Data;
using JabberwockyWorld.AnimationSystem.Editor.Elements;
using JabberwockyWorld.AnimationSystem.Scripts;
using JabberwockyWorld.AnimationSystem.Editor.Utiilites;

namespace JabberwockyWorld.AnimationSystem.Editor
{
    public class AnimationGraphView : GraphView
    {
        private AnimationEditorWindow _animationWindow;
        private AnimationSearchWindow _searchWindow;

        private SerializableDictionaryForAnimation<string, AnimationNodeErrorData> _ungroupedNodes;
        private SerializableDictionaryForAnimation<string, AnimationGroupErrorData> _groups;
        private SerializableDictionaryForAnimation<Group, SerializableDictionaryForAnimation<string, AnimationNodeErrorData>> _groupedNodes;

        private int _errorsAmount;

        public int ErrorsAmount
        {
            get
            {
                return _errorsAmount;
            }

            set
            {
                _errorsAmount = value;

                if (_errorsAmount == 0)
                {
                    _animationWindow.EnableSaving();
                }

                if (_errorsAmount == 1)
                {
                    _animationWindow.DisableSaving();
                }
            }
        }

        public AnimationGraphView(AnimationEditorWindow animationWindow)
        {
            _animationWindow = animationWindow;
            _ungroupedNodes = new SerializableDictionaryForAnimation<string, AnimationNodeErrorData>();
            _groups = new SerializableDictionaryForAnimation<string, AnimationGroupErrorData>();
            _groupedNodes = new SerializableDictionaryForAnimation<Group, SerializableDictionaryForAnimation<string, AnimationNodeErrorData>>();

            AddManipulators();
            AddGridBackground();
            AddStyles();

            AddSearchWindow();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupRenamed();
            OnGroupElementsRemoved();
            OnGraphViewChanged();
        }

        private void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<AnimationSearchWindow>();

                _searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatablePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;

                compatablePorts.Add(port);
            });

            return compatablePorts;
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<Group> groupsToDelete = new List<Group>();
                List<Edge> edgesToDelete = new List<Edge>();
                List<AnimationNodeGraph> nodesToDelete = new List<AnimationNodeGraph>();

                foreach (GraphElement element in selection)
                {
                    if (element is Group group)
                    {
                        groupsToDelete.Add(group);
                    }

                    if (element is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                    }

                    if (element is AnimationNodeGraph node)
                    {
                        nodesToDelete.Add(node);
                    }
                }

                foreach (Group group in groupsToDelete)
                {
                    List<AnimationNodeGraph> groupedNodes = new List<AnimationNodeGraph>();

                    foreach (GraphElement groupElement in group.containedElements)
                    {
                        if (!(groupElement is AnimationNodeGraph)) continue;

                        groupedNodes.Add((AnimationNodeGraph)groupElement);
                    }

                    group.RemoveElements(groupedNodes);
                    RemoveGroup((AnimationGroup)group);

                    RemoveElement(group);
                }

                DeleteElements(edgesToDelete);

                foreach (AnimationNodeGraph node in nodesToDelete)
                {
                    if (node.Group != null) node.Group.RemoveElement(node);

                    node.DisconnectAllPorts();

                    RemoveUngroupedNode(node);

                    RemoveElement(node);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is AnimationNodeGraph dialogueNode)) continue;

                    AnimationGroup dialogueGroup = (AnimationGroup)group;

                    dialogueNode.Group = dialogueGroup;

                    RemoveUngroupedNode(dialogueNode);
                    AddGroupedNode(dialogueNode, dialogueGroup);
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                AnimationGroup dialogueGroup = (AnimationGroup)group;

                if (string.IsNullOrEmpty(dialogueGroup.title))
                {
                    if (!string.IsNullOrEmpty(dialogueGroup.OldTitle))
                    {
                        ++ErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dialogueGroup.OldTitle))
                    {
                        --ErrorsAmount;
                    }
                }

                RemoveGroup(dialogueGroup);

                dialogueGroup.OldTitle = newTitle;

                AddGroup(dialogueGroup);
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is AnimationNodeGraph dialogueNode)) continue;

                    dialogueNode.Group = null;

                    RemoveGroupedNode(dialogueNode, group);
                    AddUngroupedNode(dialogueNode);
                }
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        AnimationNodeGraph nextNode = (AnimationNodeGraph)edge.input.node;

                        AnimationChoiceEditorData choiceData = (AnimationChoiceEditorData)edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge)element;

                        AnimationChoiceEditorData choiceData = (AnimationChoiceEditorData)edge.output.userData;

                        choiceData.NodeID = "";
                    }
                }

                return changes;
            };
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "AnimationSystem/AnimatorGraphViewStyle.uss",
                "AnimationSystem/AnimationNodeStyle.uss"
                );
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextManipulator("Add Animation Node", AnimationType.SimpleAnimation));
            this.AddManipulator(CreateSwitchNodeContextManipulator("Add Switch Node", AnimationType.Switch));
            this.AddManipulator(CreateGroupContextManipulator());
        }

        private IManipulator CreateNodeContextManipulator(string actionTitle, AnimationType type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("AnimationName", type, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
                ));

            return contextualMenuManipulator;
        }

        private IManipulator CreateSwitchNodeContextManipulator(string actionTitle, AnimationType type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("SwitchName", type, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
                ));

            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextManipulator()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Animation Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
                ));
            return contextualMenuManipulator;
        }

        public GraphElement CreateGroup(string title, Vector2 localMousePosition)
        {
            AnimationGroup group = new AnimationGroup(title, localMousePosition);

            foreach (GraphElement selected in selection)
            {
                if (!(selected is AnimationNodeGraph)) continue;

                AnimationNodeGraph node = (AnimationNodeGraph)selected;

                group.AddElement(node);
            }

            AddGroup(group);

            return group;
        }

        public AnimationNodeGraph CreateNode(string name, AnimationType type, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"JabberwockyWorld.AnimationSystem.Editor.Elements.{type}NodeGraph");

            AnimationNodeGraph node = (AnimationNodeGraph)Activator.CreateInstance(nodeType);

            node.Initialize(name, position, this);

            if (shouldDraw) node.Draw();

            AddUngroupedNode(node);

            return node;
        }

        public void AddUngroupedNode(AnimationNodeGraph node)
        {
            string nodeName = node.Name;

            if (!_ungroupedNodes.ContainsKey(nodeName))
            {
                AnimationNodeErrorData errorData = new AnimationNodeErrorData();
                errorData.Nodes.Add(node);

                _ungroupedNodes.Add(nodeName, errorData);

                return;
            }

            List<AnimationNodeGraph> ungroupedNodeList = _ungroupedNodes[nodeName].Nodes;
            ungroupedNodeList.Add(node);

            Color errorColor = _ungroupedNodes[nodeName].Error.Color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodeList.Count == 2)
            {
                ErrorsAmount++;

                ungroupedNodeList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(AnimationNodeGraph node)
        {
            string nodeName = node.Name;

            List<AnimationNodeGraph> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                ErrorsAmount--;

                ungroupedNodesList[0].ResetStyle();

                return;
            }

            if (ungroupedNodesList.Count == 0)
            {
                _ungroupedNodes.Remove(nodeName);
            }
        }

        private void AddGroup(AnimationGroup group)
        {
            string groupName = group.title;

            if (!_groups.ContainsKey(groupName))
            {
                AnimationGroupErrorData groupErrorData = new AnimationGroupErrorData();
                groupErrorData.Groups.Add(group);

                _groups.Add(groupName, groupErrorData);
                return;
            }

            List<AnimationGroup> groupsList = _groups[groupName].Groups;

            groupsList.Add(group);

            Color errorColor = _groups[groupName].Error.Color;

            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++ErrorsAmount;

                groupsList[0].SetErrorStyle(errorColor);
            }
        }

        private void RemoveGroup(AnimationGroup group)
        {
            string oldGroupName = group.OldTitle;

            List<AnimationGroup> groupsList = _groups[oldGroupName].Groups;

            groupsList.Remove(group);

            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --ErrorsAmount;

                groupsList[0].ResetStyle();

                return;
            }

            if (groupsList.Count == 0)
            {
                _groups.Remove(oldGroupName);
            }
        }

        public void AddGroupedNode(AnimationNodeGraph node, Group group)
        {
            string nodeName = node.Name;

            node.Group = group;

            if (!_groupedNodes.ContainsKey(group))
            {
                _groupedNodes.Add(group, new SerializableDictionaryForAnimation<string, AnimationNodeErrorData>());
            }

            if (!_groupedNodes[group].ContainsKey(nodeName))
            {
                AnimationNodeErrorData nodeErrorData = new AnimationNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                _groupedNodes[group].Add(nodeName, nodeErrorData);

                return;
            }

            List<AnimationNodeGraph> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);

            Color errorColor = _groupedNodes[group][nodeName].Error.Color;

            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++ErrorsAmount;

                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(AnimationNodeGraph node, Group group)
        {
            string nodeName = node.Name;

            node.Group = null;

            List<AnimationNodeGraph> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);

            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --ErrorsAmount;

                groupedNodesList[0].ResetStyle();

                return;
            }

            if (groupedNodesList.Count == 0)
            {
                _groupedNodes[group].Remove(nodeName);

                if (_groupedNodes[group].Count == 0) _groupedNodes.Remove(group);
            }
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isContextWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isContextWindow)
            {
                worldMousePosition -= _animationWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));

            _groups.Clear();
            _groupedNodes.Clear();
            _ungroupedNodes.Clear();

            ErrorsAmount = 0;
        }
    }
}
#endif