using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;


namespace JabberwockyWorld.DynamicEvents.Editor.Utilites
{
    public class SearchBar<T>
    {
        public T SelectedItem;
        public string Search;

        public Action<int> OnGUI;

        public virtual void DrawSearchBar(List<T> list)
        {
            EditorGUILayout.BeginHorizontal();
            Search = EditorGUILayout.TextField(Search);

            DrawButtons(list);

            EditorGUILayout.EndHorizontal();
        }

        public virtual void DrawElements(List<T> list, Action action = null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Search == null)
                {
                    if (GUILayout.Button($"{list[i]}", GUI.skin.label))
                    {
                        SelectedItem = list[i];
                        action?.Invoke();
                    }
                    OnGUI?.Invoke(i);
                }

                if (Search != null && list[i].ToString().ToLower().Contains(Search.ToLower()))
                {
                    if (GUILayout.Button($"{list[i]}", GUI.skin.label))
                    {
                        SelectedItem = list[i];
                        action?.Invoke();
                    }
                    OnGUI?.Invoke(i);
                }
            }
        }

        public void DrawButtons(List<T> list)
        {
            DrawPlusButton(list);
            DrawMinusButton(list);
        }

        public virtual void DrawPlusButton(List<T> list)
        {
            if (GUILayout.Button("+", EditorStyles.boldLabel, GUILayout.MaxWidth(30f)))
            {
                if (Search == null) Search = "";

                var created = (T)Activator.CreateInstance(typeof(T), new object[] { Search, Guid.NewGuid().ToString() });
                list.Add(created);

                Search = "";
            }
        }

        public virtual void DrawMinusButton(List<T> list)
        {
            if (GUILayout.Button("-", EditorStyles.boldLabel, GUILayout.MaxWidth(30f)))
            {
                list.Remove(SelectedItem);
            }
        }
    }
}