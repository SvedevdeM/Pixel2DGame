using System;
using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

using JabberwockyWorld.DialogueSystem.Editor.Data;
using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Editor.Utilities;
using JabberwockyWorld.DialogueSystem.Editor.Elements;

namespace JabberwockyWorld.DialogueSystem.Editor
{
    public class DialogueGraphView : GraphView
    {
        private DialogueEditorWindow _dialogueWindow;
        private DialogueSearchWindow _searchWindow;

        private SerializableDictionary<string, DialogueNodeErrorData> _ungroupedNodes;
        private SerializableDictionary<string, DialogueGroupErrorData> _groups;
        private SerializableDictionary<Group, SerializableDictionary<string, DialogueNodeErrorData>> _groupedNodes;

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
                    _dialogueWindow.EnableSaving();
                }

                if (_errorsAmount == 1)
                {
                    _dialogueWindow.DisableSaving();
                }
            }
        }

        public DialogueGraphView(DialogueEditorWindow dialogueEditor) 
        {
            _dialogueWindow = dialogueEditor;
            _ungroupedNodes = new SerializableDictionary<string, DialogueNodeErrorData>();
            _groups = new SerializableDictionary<string, DialogueGroupErrorData>();
            _groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DialogueNodeErrorData>>();

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
                _searchWindow = ScriptableObject.CreateInstance<DialogueSearchWindow>();

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
                List<GraphNode> nodesToDelete = new List<GraphNode>();

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

                    if (element is GraphNode node)
                    {
                        nodesToDelete.Add(node);
                    }
                }

                foreach (Group group in groupsToDelete)
                {
                    List<GraphNode> groupedNodes = new List<GraphNode>();

                    foreach (GraphElement groupElement in group.containedElements)
                    {
                        if (!(groupElement is GraphNode)) continue;

                        groupedNodes.Add((GraphNode)groupElement);
                    }

                    group.RemoveElements(groupedNodes);
                    RemoveGroup((GraphGroup)group);

                    RemoveElement(group);
                }

                DeleteElements(edgesToDelete);

                foreach (GraphNode node in nodesToDelete)
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
                    if(!(element is GraphNode dialogueNode)) continue;

                    GraphGroup dialogueGroup = (GraphGroup)group;

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
                GraphGroup dialogueGroup = (GraphGroup)group;

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
                    if (!(element is GraphNode dialogueNode)) continue;                

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
                        GraphNode nextNode = (GraphNode)edge.input.node;

                        DialogueChoiceEditorData choiceData = (DialogueChoiceEditorData)edge.output.userData;

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

                        DialogueChoiceEditorData choiceData = (DialogueChoiceEditorData)edge.output.userData;

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
                "DialogueSystem/DialogueGraphViewStyle.uss", 
                "DialogueSystem/DialogueNodeStyle.uss"
                );          
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextManipulator("Add Node (Single Choice Node)", DialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextManipulator("Add Node (Multiple Choice Node)", DialogueType.MultipleChoice));
            this.AddManipulator(CreateNodeContextManipulator("Add Node (Event Node)", DialogueType.Event));
            this.AddManipulator(CreateGroupContextManipulator());
        }

        private IManipulator CreateNodeContextManipulator(string actionTitle, DialogueType type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("Node Name", type, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
                ));
            
            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextManipulator()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
                ));

            return contextualMenuManipulator;
        }

        public GraphElement CreateGroup(string title, Vector2 localMousePosition)
        {
            GraphGroup group = new GraphGroup(title, localMousePosition);

            AddGroup(group);
            AddElement(group);

            foreach (GraphElement selected in selection)
            {
                if (!(selected is GraphNode)) continue;

                GraphNode node = (GraphNode)selected;

                group.AddElement(node);
            }

            return group;
        }

        public GraphNode CreateNode(string name, DialogueType type, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"JabberwockyWorld.DialogueSystem.Editor.Elements.{type}Node");

            GraphNode node = (GraphNode)Activator.CreateInstance(nodeType);

            node.Initialize(name, position, this);

            if (shouldDraw) node.Draw();

            AddUngroupedNode(node);

            return node;
        }

        public void AddUngroupedNode(GraphNode node)
        {
            string nodeName = node.Name;

            if (!_ungroupedNodes.ContainsKey(nodeName))
            {
                DialogueNodeErrorData errorData = new DialogueNodeErrorData();
                errorData.Nodes.Add(node);

                _ungroupedNodes.Add(nodeName, errorData);

                return;
            }

            List<GraphNode> ungroupedNodeList = _ungroupedNodes[nodeName].Nodes;
            ungroupedNodeList.Add(node);

            Color errorColor = _ungroupedNodes[nodeName].Error.Color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodeList.Count == 2)
            {
                ErrorsAmount++;

                ungroupedNodeList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(GraphNode node)
        {
            string nodeName = node.Name;

            List<GraphNode> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

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

        private void AddGroup(GraphGroup group)
        {
            string groupName = group.title;

            if (!_groups.ContainsKey(groupName))
            {
                DialogueGroupErrorData groupErrorData = new DialogueGroupErrorData();
                groupErrorData.Groups.Add(group);

                _groups.Add(groupName, groupErrorData);
                return;
            }

            List<GraphGroup> groupsList = _groups[groupName].Groups;

            groupsList.Add(group);

            Color errorColor = _groups[groupName].Error.Color;

            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++ErrorsAmount;

                groupsList[0].SetErrorStyle(errorColor);
            }
        }

        private void RemoveGroup(GraphGroup group)
        {
            string oldGroupName = group.OldTitle;

            List<GraphGroup> groupsList = _groups[oldGroupName].Groups;

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

        public void AddGroupedNode(GraphNode node, Group group)
        {
            string nodeName = node.Name;

            node.Group = group;

            if (!_groupedNodes.ContainsKey(group))
            {
                _groupedNodes.Add(group, new SerializableDictionary<string, DialogueNodeErrorData>());
            }

            if (!_groupedNodes[group].ContainsKey(nodeName))
            {
                DialogueNodeErrorData nodeErrorData = new DialogueNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                _groupedNodes[group].Add(nodeName, nodeErrorData);

                return;
            }

            List<GraphNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);

            Color errorColor = _groupedNodes[group][nodeName].Error.Color;

            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++ErrorsAmount;

                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(GraphNode node, Group group)
        {
            string nodeName = node.Name;

            node.Group = null;

            List<GraphNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

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

            if(isContextWindow)
            {
                worldMousePosition -= _dialogueWindow.position.position;
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
