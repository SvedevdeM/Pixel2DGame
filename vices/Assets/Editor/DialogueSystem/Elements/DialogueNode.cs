using JabberwockyWorld.DialogueSystem.Editor.Utilities;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Search;
using JabberwockyWorld.DialogueSystem.Scripts.Data;

namespace JabberwockyWorld.DialogueSystem.Editor.Elements
{
    public class DialogueNode : GraphNode
    {
        protected override void OnInitialize(string name, Vector2 position, DialogueGraphView graphView) { }
        protected override void OnDraw()
        {
            Foldout foldout = DialogueElementUtility.CreateFoldout("Text");

            TextField textField = DialogueElementUtility.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });

            textField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
                );

            ObjectField actorField = new ObjectField()
            {
                objectType = typeof(ActorInfo),
            };

            actorField.SetValueWithoutNotify(Actor);
            actorField.RegisterValueChangedCallback((value) => Actor = (ActorInfo)value.newValue);

            _customData.Add(actorField);

            foldout.Add(textField);
            _customData.Add(foldout);
        }
    }
}