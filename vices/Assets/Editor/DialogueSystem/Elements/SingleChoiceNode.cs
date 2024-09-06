using JabberwockyWorld.DialogueSystem.Editor.Utilities;
using JabberwockyWorld.DialogueSystem.Scripts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor.Elements
{
    public class SingleChoiceNode : DialogueNode
    {
        protected override void OnInitialize(string name, Vector2 position, DialogueGraphView graphView)
        {
            Type = DialogueType.SingleChoice;
        }

        protected override void OnDraw()
        {
            base.OnDraw();

            for (int i = 0; i < Choices.Count; i++)
            {
                Port choicePort = DialogueElementUtility.CreatePort(this, Choices[i].Text);

                choicePort.userData = Choices[i];

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}