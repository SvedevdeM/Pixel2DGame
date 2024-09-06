using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts;
using JabberwockyWorld.DynamicEvents.Editor.Utilites;
using JabberwockyWorld.DynamicEvents.Scripts.Data;


namespace JabberwockyWorld.DynamicEvents.Editor.Drawers
{
    public class CriteriasEditorDrawer
    {
        public List<Criteria> Criterias;
        public List<FactEntry> Facts;
        public int ItemIndex;

        public SearchBar<Criteria> CriteriaSearch = new SearchBar<Criteria>();

        private EventDatabase _database;
        public void SetDatabase(EventDatabase database)
        {
            _database = database;
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Criterias:");
            CriteriaSearch.DrawPlusButton(Criterias);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            for (int i = 0; i < Criterias.Count; i++)
            {
                if (Criterias.Count < 0) break;

                EditorGUILayout.BeginHorizontal();
                if (Criterias[i].Fact != null)
                {
                    EditorGUILayout.LabelField(Criterias[i].Fact.Name);
                }
                else
                    EditorGUILayout.LabelField("NONE", EditorStyles.boldLabel);

                Criterias[i].Type = (Criteria.EqualType)EditorGUILayout.EnumPopup(Criterias[i].Type);
                Criterias[i].Value = EditorGUILayout.IntField(Criterias[i].Value, GUILayout.MaxWidth(80f));

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUI.skin.label))
                {
                    var eventTablesWindow = EditorExtentions.CreateSearchWindow(typeof(EventTable));
                    eventTablesWindow.SetValues(_database.EventTables, (s) => {
                        var window = EditorExtentions.CreateSearchWindowBaseEntry(typeof(FactEntry));
                        window.SetValues(s.Facts, (selected) =>
                        {
                            Criterias[ItemIndex].Fact = selected as FactEntry;
                            window.Close();
                        });
                    });

                    ItemIndex = i;
                }

                if (GUILayout.Button("-", GUI.skin.label))
                {
                    Criterias.Remove(Criterias[i]);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        public void SetValues(List<Criteria> criterias, List<FactEntry> factEntries)
        {
            Criterias = criterias;
            Facts = factEntries;
        }
    }
}