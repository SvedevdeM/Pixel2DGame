using JabberwockyWorld.DialogueSystem.Editor.Data;
using JabberwockyWorld.DialogueSystem.Editor.Utilities;
using JabberwockyWorld.DialogueSystem.Scripts;

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace JabberwockyWorld.DialogueSystem.Editor.Elements
{
    public class MultipleChoiceNode : DialogueNode
    {
        protected override void OnInitialize(string name, Vector2 position, DialogueGraphView graphView)
        {
            Type = DialogueType.MultipleChoice;
        }

        protected override void OnDraw()
        {
            base.OnDraw();

            Button addChoiceButton = DialogueElementUtility.CreateButton("Add Choice", () =>
            {
                DialogueChoiceEditorData choice = new DialogueChoiceEditorData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choice);

                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            for (int i = 0; i < Choices.Count; i++)
            {
                Port choicePort = CreateChoicePort(Choices[i]);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        private Port CreateChoicePort(object userData)
        {
            Port choicePort = DialogueElementUtility.CreatePort(this);

            choicePort.userData = userData;

            DialogueChoiceEditorData choiceData = (DialogueChoiceEditorData)userData;

            Button deleteChoiceButton = DialogueElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1) return;

                if (choicePort.connected)
                {
                    _graphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(choiceData);

                _graphView.RemoveElement(choicePort);
            });

            deleteChoiceButton.AddToClassList("ds-node-button");

            TextField choiceTextField = DialogueElementUtility.CreateTextArea(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__choice-text-field",
                "ds-node__text-field__hidden"
                );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }
    }
}