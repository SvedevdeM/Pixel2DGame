
#if UNITY_EDITOR
using JabberwockyWorld.AnimationSystem.Editor;
using JabberwockyWorld.AnimationSystem.Editor.Data;
using JabberwockyWorld.AnimationSystem.Editor.Utiilites;
using JabberwockyWorld.AnimationSystem.Scripts;
using System.Collections;
using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Search;

namespace JabberwockyWorld.AnimationSystem.Editor.Elements
{
    public class SwitchNodeGraph : AnimationNodeGraph
    {
        // Start is called before the first frame update
        public override void Initialize(string name, Vector2 position, AnimationGraphView graphView)
        {
            Choices = new List<AnimationChoiceEditorData>();
            base.Initialize(name, position, graphView);

            Type = AnimationType.Switch;

            AnimationChoiceEditorData choice = new AnimationChoiceEditorData()
            {
                Text = "Next Animation"
            };

            //Controll = new Controll();

            for (int i = 0; i < Choices.Count; i++)
            {
                Port choicePort = CreateChoicePort(Choices[i]);

                outputContainer.Add(choicePort);
            }

            Choices.Add(choice);

            RefreshExpandedState();
        }

        public override void Draw()
        {
            base.Draw();

            ObjectField controllerField = new ObjectField()
            {
                objectType = typeof(Controll),
            };

            controllerField.SetValueWithoutNotify(Controll);
            controllerField.RegisterValueChangedCallback((value) => Controll = (Controll)value.newValue);

            Button addChoiceButton = AnimationElementUtility.CreateButton("Add Choice", () =>
            {
                AnimationChoiceEditorData choice = new AnimationChoiceEditorData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choice);

                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("as-node__button");

            mainContainer.Insert(1, controllerField);
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
            Port choicePort = AnimationElementUtility.CreatePort(this);

            choicePort.userData = userData;

            AnimationChoiceEditorData choiceData = (AnimationChoiceEditorData)userData;

            Button deleteChoiceButton = AnimationElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1) return;

                if (choicePort.connected)
                {
                    _graphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(choiceData);

                _graphView.RemoveElement(choicePort);
            });

            deleteChoiceButton.AddToClassList("as-node-button");

            TextField choiceTextField = AnimationElementUtility.CreateTextArea(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            choiceTextField.AddClasses(
                "as-node__text-field",
                "as-node__choice-text-field",
                "as-node__text-field__hidden"
                );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }
    }
}
#endif