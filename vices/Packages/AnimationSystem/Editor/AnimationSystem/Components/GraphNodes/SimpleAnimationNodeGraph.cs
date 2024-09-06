#if UNITY_EDITOR
using JabberwockyWorld.AnimationSystem.Editor.Utiilites;
using JabberwockyWorld.AnimationSystem.Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Search;
using JabberwockyWorld.AnimationSystem.Editor.Data;

namespace JabberwockyWorld.AnimationSystem.Editor.Elements
{
    public class SimpleAnimationNodeGraph : AnimationNodeGraph
    {

        public override void Initialize(string name, Vector2 position, AnimationGraphView graphView)
        {
            base.Initialize(name, position, graphView);

            Type = AnimationType.SimpleAnimation;
            //Cells = new List<AnimationCellEditorData>();

            /*for (int i = 0; i < Choices.Count; i++)
            {
                Port choicePort = CreateChoicePort(Choices[i]);

                outputContainer.Add(choicePort);
            }*/
        }
        
        public override void Draw()
        {
            base.Draw();

            Foldout foldout = AnimationElementUtility.CreateFoldout("Cells");

            Button addCellButton = AnimationElementUtility.CreateButton("Add Cells", () =>
            {
                AnimationCellEditorData cell = new AnimationCellEditorData();

                Cells.Add(cell);

                VisualElement fieldContainer = CreateCellObj(cell);
                foldout.Add(fieldContainer);
            });

            VisualElement customData = new VisualElement();

            customData.AddToClassList("as-node__custom-data-container");

            for (int i = 0; i < Cells.Count; i++)
            {
                VisualElement newContainer = CreateCellObj(Cells[i]);
                foldout.Add(newContainer);
            }

            customData.Add(addCellButton);
            customData.Add(foldout);

            extensionContainer.Add(customData);

            RefreshExpandedState();
        }

        private VisualElement CreateCellObj(AnimationCellEditorData cell)
        {
            ObjectField newCellField = new ObjectField()
            {
                objectType = typeof(Sprite),
            };

            VisualElement fieldContainer = new VisualElement();

            newCellField.SetValueWithoutNotify(cell.Sprite);
            newCellField.RegisterValueChangedCallback((value) => cell.Sprite = (Sprite)value.newValue);

            Button deletecellbutton = AnimationElementUtility.CreateButton("X", () =>
            {
                if (Cells.Count < 1) return;

                Cells.Remove(cell);
                fieldContainer.Remove(newCellField);
                //fieldContainer.Remove(deletecellbutton);
                //extensionContainer.Remove(fieldContainer);
                fieldContainer.Clear();
                //_graphView.Remove(fieldContainer);
                //_graphView.Remove(newCellField);
                RefreshExpandedState();
            });

            deletecellbutton.AddToClassList("as-node-button");

            fieldContainer.style.flexDirection = FlexDirection.Row;
            fieldContainer.Add(newCellField);
            fieldContainer.Add(deletecellbutton);

            //CreateDriverFold();?

            return fieldContainer;

        }
    }
}
#endif