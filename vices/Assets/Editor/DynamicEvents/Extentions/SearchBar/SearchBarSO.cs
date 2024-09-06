using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System;

namespace JabberwockyWorld.DynamicEvents.Editor.Utilites
{
    public class SearchBarSO<T> : SearchBar<T> where T : ScriptableObject
    {
        public override void DrawSearchBar(List<T> list)
        {
            EditorGUILayout.BeginHorizontal();

            Search = EditorGUILayout.TextField(Search);

            if (GUILayout.Button("+", EditorStyles.boldLabel))
            {
                var scriptableObject = ScriptableObject.CreateInstance<T>();

                scriptableObject.name = Search;
                if (Search == "") scriptableObject.name = $"{scriptableObject.GetType().Name}";

                list.Add(scriptableObject);

                Search = "";
            }

            if (GUILayout.Button("-", EditorStyles.boldLabel))
            {
                list.Remove(SelectedItem);
            }

            EditorGUILayout.EndHorizontal();
        }

        public override void DrawElements(List<T> list, Action action = null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Search == null)
                {
                    if (GUILayout.Button($"{(list[i] as ScriptableObject).name}", GUI.skin.label))
                    {
                        SelectedItem = list[i];
                        action?.Invoke();
                    }
                    OnGUI?.Invoke(i);
                }

                if (Search != null && list[i].ToString().ToLower().Contains(Search.ToLower()))
                {
                    if (GUILayout.Button($"{(list[i] as ScriptableObject).name}", GUI.skin.label))
                    {
                        SelectedItem = list[i];
                        action?.Invoke();
                    }
                    OnGUI?.Invoke(i);
                }
            }
        }
    }
}