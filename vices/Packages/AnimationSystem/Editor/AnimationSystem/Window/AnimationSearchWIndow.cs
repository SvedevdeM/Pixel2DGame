#if UNITY_EDITOR 
using JabberwockyWorld.AnimationSystem;
using JabberwockyWorld.AnimationSystem.Editor.Elements;
using JabberwockyWorld.AnimationSystem.Scripts;
using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;
using UnityEngine;
#endif
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor
{
    public class AnimationSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private AnimationGraphView _graphView;
        private Texture2D _indentationIcon;

        public void Initialize(AnimationGraphView graphView)
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
            new SearchTreeGroupEntry(new GUIContent("Animation Node"), 1),
            new SearchTreeEntry(new GUIContent("Simple Animation", _indentationIcon))
            {
                level = 2,
                userData = AnimationType.SimpleAnimation
            },
            new SearchTreeEntry(new GUIContent("Switch Node", _indentationIcon))
            {
                level = 2,
                userData = AnimationType.Switch
            },

            new SearchTreeGroupEntry(new GUIContent("Animation Group"), 1),
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
                case AnimationType.SimpleAnimation:
                    {
                        SimpleAnimationNodeGraph simpAnimation = (SimpleAnimationNodeGraph)_graphView.CreateNode("Animation Name", AnimationType.SimpleAnimation, localPosition);

                        _graphView.AddElement(simpAnimation);

                        return true;
                    }
                case AnimationType.Switch:
                    {
                        SwitchNodeGraph switchNode = (SwitchNodeGraph)_graphView.CreateNode("Switch Name", AnimationType.Switch, localPosition);

                        _graphView.AddElement(switchNode);

                        return true;
                    }
                case Group _:
                    {
                        Group group = (Group)_graphView.CreateGroup("Animation Group", localPosition);

                        _graphView.AddElement(group);

                        return true;
                    }
                default: return false;
            }
        }
    }
}
#endif