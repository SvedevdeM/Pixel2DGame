using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using JabberwockyWorld.DynamicEvents.Editor.Windows;


namespace JabberwockyWorld.DynamicEvents.Editor.Utilites
{
    public static class EditorExtentions
    {
        public static SearchWindowWrapper CreateSearchWindow<T>(T type) where T : System.Type
        {
            return SearchWindowWrapper.Open(type);
        }

        public static SearchWindowBaseEntryWrapper CreateSearchWindowBaseEntry<T>(T type) where T : System.Type
        {
            return SearchWindowBaseEntryWrapper.Open(type);
        }

        public static void DrawSearchBarInformation<T>(SearchBar<T> searchBar, List<T> list, string text, Action action = null)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginFoldoutHeaderGroup(true, text);
            searchBar.DrawSearchBar(list);            
            searchBar.DrawElements(list, action);
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.EndVertical();
        }

        public static void DrawAsDropDown(this EditorWindow window)
        {
            Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Rect rect = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
            window.ShowAsDropDown(rect, new Vector3(500, 300));
        }

        public static void DrawFoldoutGroup(string name)
        {
            EditorGUILayout.BeginFoldoutHeaderGroup(true, name);
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        public static void Space(int width = 5, bool isExpand = true)
        {
            EditorGUILayout.Space(width, isExpand);
        }
    }
}
