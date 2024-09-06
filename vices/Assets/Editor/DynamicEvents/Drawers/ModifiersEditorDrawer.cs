using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts;
using JabberwockyWorld.DynamicEvents.Editor.Utilites;
using JabberwockyWorld.DynamicEvents.Scripts.Data;


namespace JabberwockyWorld.DynamicEvents.Editor.Drawers
{
    public class ModifiersEditorDrawer
    {
        public List<Modifier> Modifiers;
        public List<FactEntry> Facts;
        public int ItemIndex;

        public SearchBar<Modifier> ModifierSearch = new SearchBar<Modifier>();

        private EventDatabase _database;
        public void SetDatabase(EventDatabase database)
        {
            _database = database;
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Modifiers:");
            ModifierSearch.DrawPlusButton(Modifiers);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            for (int i = 0; i < Modifiers.Count; i++)
            {
                if (Modifiers.Count < 0) break;

                EditorGUILayout.BeginHorizontal();
                if (Modifiers[i].Fact != null)
                {
                    EditorGUILayout.LabelField(Modifiers[i].Fact.Name);
                }
                else
                    EditorGUILayout.LabelField("NONE", EditorStyles.boldLabel);

                Modifiers[i].Type = (Modifier.ModifierType)EditorGUILayout.EnumPopup(Modifiers[i].Type);
                Modifiers[i].Value = EditorGUILayout.IntField(Modifiers[i].Value, GUILayout.MaxWidth(80f));

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUI.skin.label))
                {
                    var eventTablesWindow = EditorExtentions.CreateSearchWindow(typeof(EventTable));
                    eventTablesWindow.SetValues(_database.EventTables, (s) => {
                        var window = EditorExtentions.CreateSearchWindowBaseEntry(typeof(FactEntry));
                        window.SetValues(s.Facts, (selected) =>
                        {
                            Modifiers[ItemIndex].Fact = selected as FactEntry;                          
                            window.Close();
                        });
                    });

                    ItemIndex = i;
                }

                if (GUILayout.Button("-", GUI.skin.label))
                {
                    Modifiers.Remove(Modifiers[i]);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        public void SetValues(List<Modifier> modifiers, List<FactEntry> factEntries)
        {
            Modifiers = modifiers;
            Facts = factEntries;
        }
    }
}