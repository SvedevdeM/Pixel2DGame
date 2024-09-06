using JabberwockyWorld.DialogueSystem.Editor.Elements;
using JabberwockyWorld.DialogueSystem.Scripts;

using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor
{
    public class DialogueSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView _graphView;
        private Texture2D _indentationIcon;

        public void Initialize(DialogueGraphView graphView)
        {
            _graphView = graphView;

            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, Color.clear);
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Create Element")),
            new SearchTreeGroupEntry(new GUIContent("Graph Node"), 1),
            new SearchTreeEntry(new GUIContent("Single Choice", _indentationIcon))
            {
                level = 2,
                userData = DialogueType.SingleChoice
            },
            new SearchTreeEntry(new GUIContent("Multiple Choice", _indentationIcon))
            {
                level = 2,
                userData = DialogueType.MultipleChoice
            },
            new SearchTreeEntry(new GUIContent("Event", _indentationIcon))
            {
                level = 2,
                userData = DialogueType.Event
            },
            new SearchTreeGroupEntry(new GUIContent("Graph Group"), 1),
            new SearchTreeEntry(new GUIContent("Group", _indentationIcon))
            {
                level = 2,
                userData = new Group()
            },
        };

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localPosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (SearchTreeEntry.userData)
            {
                case DialogueType.SingleChoice:
                    {
                        SingleChoiceNode singleChoiceNode = (SingleChoiceNode)_graphView.CreateNode("DialogueName", DialogueType.SingleChoice, localPosition);

                        _graphView.AddElement(singleChoiceNode);

                        return true;
                    }
                case DialogueType.MultipleChoice:
                    {
                        MultipleChoiceNode multipleChoiceNode = (MultipleChoiceNode)_graphView.CreateNode("DialogueName", DialogueType.MultipleChoice, localPosition);

                        _graphView.AddElement(multipleChoiceNode);

                        return true;
                    }
                case Group _:
                    {
                        Group group = (Group)_graphView.CreateGroup("Dialogue Group", localPosition);

                        _graphView.AddElement(group);

                        return true;
                    }
                default: return false;
            }
        }
    }
}
