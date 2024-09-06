#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

using System;
using System.Linq;
using JabberwockyWorld.AnimationSystem.Editor.Data;
using JabberwockyWorld.AnimationSystem.Scripts;
using JabberwockyWorld.AnimationSystem.Editor.Utiilites;
using UnityEditor.Search;

namespace JabberwockyWorld.AnimationSystem.Editor
{
    public class AnimationNodeGraph : Node
    {
        public string ID { get; set; }
        public Group Group { get; set; }
        public string Name { get; set; }
        public Controll Controll { get; set; }
        public List<AnimationChoiceEditorData> Choices { get; set; }
        public List<AnimationDriverDIctEditorData> Drivers { get; set; }
        public List<AnimationCellEditorData> Cells {  get; set; }
        public AnimationType Type { get; set; }

        private Color _defaultColor;

        protected AnimationGraphView _graphView;

        public virtual void Initialize(string name, Vector2 position, AnimationGraphView graphView)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            _defaultColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            Drivers = new List<AnimationDriverDIctEditorData>();
            Cells = new List<AnimationCellEditorData>();
            _graphView = graphView;
            Choices = new List<AnimationChoiceEditorData>();

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("as-node__main-container");
            extensionContainer.AddToClassList("as-node__extension-container");
        }

        public virtual void Draw()
        {
            TextField animationNameField = AnimationElementUtility.CreateTextField(Name, null, callback =>
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
            animationNameField.AddClasses(
                "as-node__text-field",
                "as-node__filename-text-field",
                "as-node__textField__hidden"
                );

            titleContainer.Add(animationNameField);

            Port inputPort = AnimationElementUtility.CreatePort(this, "Input Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            VisualElement customData = new VisualElement();

            customData.AddToClassList("as-node__custom-data-container");

            extensionContainer.Add(customData);

            VisualElement driverWOrk = CreateDriverFold();

            for (int i = 0; i < Drivers.Count; i++)
            {
                VisualElement driver = CreateDriverObj(Drivers[i]);
                driverWOrk.Add(driver);
            }

            extensionContainer.Add(driverWOrk);
            RefreshExpandedState();
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port)inputContainer.Children().First();

            return !inputPort.connected;
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

        public VisualElement CreateDriverFold()
        {
            Foldout foldout = AnimationElementUtility.CreateFoldout("Drivers");

            Button addDriverButton = AnimationElementUtility.CreateButton("Add Driver", () =>
            {
                AnimationDriverDIctEditorData driver = new AnimationDriverDIctEditorData()
                {
                     Key = 0
                };

                Drivers.Add(driver);
                CreateDriverObj(driver);

                VisualElement fieldContainer = CreateDriverObj(driver);
                foldout.Add(fieldContainer);
            });

            VisualElement customData = new VisualElement();

            customData.AddToClassList("as-node__custom-data-container");

            customData.Add(addDriverButton);
            customData.Add(foldout);

            return customData;
        }

        private VisualElement CreateDriverObj(AnimationDriverDIctEditorData driver)
        {
            ObjectField newControllerField = new ObjectField()
            {
                objectType = typeof(Controll),
            };

            VisualElement fieldContainer = new VisualElement();

            newControllerField.SetValueWithoutNotify(driver.Controll);
            newControllerField.RegisterValueChangedCallback((value) => driver.Controll = (Controll)value.newValue);

            Button deletedriverbutton = AnimationElementUtility.CreateButton("X", () =>
            {
                if (Drivers.Count < 1) return;

                Drivers.Remove(driver);
                fieldContainer.Clear();
                RefreshExpandedState();
            });

            deletedriverbutton.AddToClassList("as-node-button");

            TextField driverKeyField = AnimationElementUtility.CreateIntegerTextArea(0, null, callback =>
            {
                TextField target = (TextField)callback.target;
            });
            driverKeyField.AddClasses(
                "as-node__text-field"
                );

            driverKeyField.SetValueWithoutNotify(driver.Key.ToString());
            driverKeyField.RegisterValueChangedCallback((value) => driver.Key = int.Parse(value.newValue));

            fieldContainer.style.flexDirection = FlexDirection.Row;
            fieldContainer.Add(newControllerField);
            fieldContainer.Add(driverKeyField);
            fieldContainer.Add(deletedriverbutton);

            return fieldContainer;

        }
    }
}
#endif