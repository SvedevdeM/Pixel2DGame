using System;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;
using JabberwockyWorld.AnimationSystem.Editor;
using UnityEngine;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor.Utiilites
{
    public static class AnimationElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Port CreatePort(AnimationNodeGraph node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = CreateTextField(value, label, onValueChanged);

            textField.multiline = false;

            return textField;
        }

        public static TextField CreateIntegerTextArea(int value = 0, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = CreateIntegerField(value, label, onValueChanged);

            textField.multiline = true;

            return textField;
        }

        public static TextField CreateIntegerField(int value = 0, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value.ToString(),
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(evt =>
                {
                    try
                    {
                        int intValue = int.Parse(evt.newValue);
                        onValueChanged.Invoke(evt);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Error parsing integer value: {ex.Message}");
                    }
                });
            }

            return textField;
        }
    }
}
#endif