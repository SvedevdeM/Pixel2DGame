using JabberwockyWorld.DialogueSystem.Editor.Utilities;
using JabberwockyWorld.DialogueSystem.Scripts;
using JabberwockyWorld.DynamicEvents.Scripts;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JabberwockyWorld.DialogueSystem.Editor.Elements
{
    public class EventNode : GraphNode
    {
        protected override void OnInitialize(string name, Vector2 position, DialogueGraphView graphView)
        {
            Type = DialogueType.Event;
        }

        protected override void OnDraw()
        {
            for (int i = 0; i < Choices.Count; i++)
            {
                Port choicePort = DialogueElementUtility.CreatePort(this, Choices[i].Text);

                choicePort.userData = Choices[i];

                outputContainer.Add(choicePort);
            }

            ObjectField eventField = new ObjectField()
            {
                objectType = typeof(EventData),
            };

            eventField.SetValueWithoutNotify(Event);
            eventField.RegisterValueChangedCallback((value) => Event = (EventData)value.newValue);

            _customData.Add(eventField);
        }
    }
}