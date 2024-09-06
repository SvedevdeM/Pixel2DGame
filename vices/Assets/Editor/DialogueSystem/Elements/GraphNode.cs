using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DialogueSystem.Editor.Utilities;
using JabberwockyWorld.DialogueSystem.Editor.Data;

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

using System.Collections.Generic;
using System;
using System.Linq;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Scripts;

namespace JabberwockyWorld.DialogueSystem.Editor.Elements
{
    public abstract class GraphNode : Node
    {
        public Group Group { get; set; }
        public ActorInfo Actor { get; set; }
        public EventData Event { get; set; }
        public List<DialogueChoiceEditorData> Choices { get; set; }
        public DialogueType Type { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsStartingNode
        {
            get
            {
                Port inputPort = (Port)inputContainer.Children().First();

                return !inputPort.connected;
            }
            set { }
        }

        private Color _defaultColor;
        protected DialogueGraphView _graphView;
        protected VisualElement _customData;

        protected abstract void OnInitialize(string name, Vector2 position, DialogueGraphView graphView);
        public virtual void Initialize(string name, Vector2 position, DialogueGraphView graphView)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Choices = new List<DialogueChoiceEditorData>();
            Text = "Some text of the dialogue";
            _defaultColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            DialogueChoiceEditorData choice = new DialogueChoiceEditorData()
            {
                Text = "Next Node"
            };

            Choices.Add(choice);

            _graphView = graphView;
            _customData = new VisualElement();
            _customData.AddToClassList("ds-node__custom-data-container");

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");

            OnInitialize(name, position, graphView);
        }

        protected abstract void OnDraw();
        public virtual void Draw()
        {
            TextField nodeNameField = DialogueElementUtility.CreateTextField(Name, null, callback =>
            {
                TextField target = (TextField)callback.target;

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        ++_graphView.ErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        --_graphView.ErrorsAmount;
                    }
                }

                if (Group == null)
                {
                    _graphView.RemoveUngroupedNode(this);

                    Name = callback.newValue;

                    _graphView.AddUngroupedNode(this);
                }
                else
                {
                    Group currentGroup = Group;

                    _graphView.RemoveGroupedNode(this, Group);

                    Name = callback.newValue;

                    _graphView.AddGroupedNode(this, currentGroup);
                }
            });

            nodeNameField.AddClasses(
                "ds-node__text-field",
                "ds-node__filename-text-field",
                "ds-node__textField__hidden"
            );

            titleContainer.Add(nodeNameField);

            Port inputPort = DialogueElementUtility.CreatePort(this, "Input Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            OnDraw();

            extensionContainer.Add(_customData);
            RefreshExpandedState();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPorts(inputContainer));
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPorts(outputContainer));

            base.BuildContextualMenu(evt);
        }

        public void DisconnectAllPorts()
        {
            DisconnectPorts(outputContainer);
            DisconnectPorts(inputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected) continue;

                _graphView.DeleteElements(port.connections);
            }
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defaultColor;
        }
    }
}