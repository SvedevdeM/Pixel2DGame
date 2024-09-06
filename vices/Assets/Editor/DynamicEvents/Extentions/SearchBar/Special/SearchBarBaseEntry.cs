using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;


namespace JabberwockyWorld.DynamicEvents.Editor.Utilites
{
    public class SearchBarBaseEntry<T> : SearchBar<T> where T : BaseEntry
    {
        public override void DrawElements(List<T> list, Action action = null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Search == null)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button($"{list[i].Name}", GUI.skin.label))
                    {
                        SelectedItem = list[i];
                        action?.Invoke();
                    }
                    OnGUI?.Invoke(i);
                    EditorGUILayout.EndHorizontal();
                }

                if (Search != null && list[i].Name.ToLower().Contains(Search.ToLower()))
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button($"{list[i].Name}", GUI.skin.label))
                    {
                        SelectedItem = list[i];
                        action?.Invoke();
                    }
                    OnGUI?.Invoke(i);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}
