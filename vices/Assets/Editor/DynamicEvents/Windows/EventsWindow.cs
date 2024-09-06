using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using JabberwockyWorld.DynamicEvents.Scripts.Enumerators;
using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Scripts;
using JabberwockyWorld.DynamicEvents.Editor.Utilites;
using JabberwockyWorld.DynamicEvents.Editor.Drawers;
using JabberwockyWorld.Editor.Utils;
using System;
using JabberwockyWorld.DialogueSystem.Scripts.Data;
using JabberwockyWorld.Quest.Scripts;


namespace JabberwockyWorld.DynamicEvents.Editor.Windows
{
    public class EventsWindow : EditorWindow
    {
        public SerializedObject SerializedObject;
        public EventDatabase EventDatabase;
        public EventTable SelectedTable;

        #region Editor classes
        public SearchBarSO<EventTable> TablesSearch = new SearchBarSO<EventTable>();
        public SearchBarBaseEntry<FactEntry> FactsSearch = new SearchBarBaseEntry<FactEntry>();
        public SearchBarBaseEntry<EventEntry> EventsSearch = new SearchBarBaseEntry<EventEntry>();
        public SearchBarBaseEntry<RuleEntry> RulesSearch = new SearchBarBaseEntry<RuleEntry>();
        public SearchBarBaseEntry<EventEntry> RuleEventSearch = new SearchBarBaseEntry<EventEntry>();
        public CriteriasEditorDrawer CriteriaDrawer = new CriteriasEditorDrawer();
        public ModifiersEditorDrawer ModifiersDrawer = new ModifiersEditorDrawer();
        #endregion

        private Vector2 _scrollPosition;
        private int _selected = 0;
        private bool _isCriterias = true;
        private bool _isModifiers;

        [MenuItem("JabberwockyWorld/Window/Events")]
        public static void Open()
        {
            var eventDatabas = Resources.Load<EventDatabase>(EditorConstants.EventDatabase + "/EventDatabase");
            if (eventDatabas == null)
            {
                Debug.LogError($"Can't find {typeof(EventDatabase)} for {typeof(EventsWindow)} on {EditorConstants.EventDatabase}/EventDatabase path");
                return;
            }

            var window = GetWindow<EventsWindow>($"Events Window");
            window.EventDatabase = eventDatabas;
            window.CriteriaDrawer.SetDatabase(eventDatabas);
            window.ModifiersDrawer.SetDatabase(eventDatabas);
        }

        private void OnGUI()
        {
            if (!EventDatabase)
            {
                EditorGUILayout.HelpBox($"Window didn't find {typeof(EventDatabase)}. \n Please create this and reopen window.", MessageType.Error);
                return;
            }

            SerializedObject = new SerializedObject(this);

            EditorGUILayout.BeginHorizontal("box");

            DrawAllTables();
            DrawTable();
            DrawInformation();

            EditorGUILayout.EndHorizontal();

            SerializedObject.ApplyModifiedProperties();

            if (!SelectedTable) return;
            EditorUtility.SetDirty(SelectedTable);
        }

        #region Main Draw Methods
        private void DrawAllTables()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Height(position.height - 18f), GUILayout.MaxWidth(150f));

            if (EventDatabase.EventTables.Count <= 0)
            {
                EditorGUILayout.HelpBox($"{typeof(EventDatabase)} didn't have any {typeof(EventTable)}.", MessageType.Warning);
                EditorGUILayout.EndVertical();
                return;
            }

            EditorExtentions.DrawSearchBarInformation(TablesSearch, EventDatabase.EventTables, "Event Tables");

            EditorGUILayout.EndVertical();
        }

        private void DrawTable()
        {
            SelectedTable = TablesSearch.SelectedItem;
            EditorGUILayout.BeginVertical("box", GUILayout.Height(position.height - 18f), GUILayout.MinWidth(150f), GUILayout.MaxWidth(250f));

            EditorExtentions.DrawFoldoutGroup("Data");

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.MinWidth(100), GUILayout.MaxWidth(250f), GUILayout.Height(position.height - 18f));

            if (SelectedTable == null)
            {
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.BeginVertical();

            EditorExtentions.DrawSearchBarInformation(FactsSearch, SelectedTable.Facts, "Facts", () => { EventsSearch.SelectedItem = null; RulesSearch.SelectedItem = null; });
            EditorExtentions.Space(5);
            EditorExtentions.DrawSearchBarInformation(EventsSearch, SelectedTable.Events, "Events", () => { FactsSearch.SelectedItem = null; RulesSearch.SelectedItem = null; });
            EditorExtentions.Space(5);
            EditorExtentions.DrawSearchBarInformation(RulesSearch, SelectedTable.Rules, "Rules", () => { EventsSearch.SelectedItem = null; FactsSearch.SelectedItem = null; });

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private void DrawInformation()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Height(position.height - 18f));
            EditorExtentions.DrawFoldoutGroup("Information");

            //Draw Fact
            DrawEntryInfo(FactsSearch.SelectedItem);
            if (FactsSearch.SelectedItem != null)
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField($"Value: {FactsSearch.SelectedItem.Value}");
                if (GUILayout.Button("+", GUI.skin.label, GUILayout.Width(30f))) FactsSearch.SelectedItem.Value++;
                if (GUILayout.Button("-", GUI.skin.label, GUILayout.Width(30f))) FactsSearch.SelectedItem.Value--;
                EditorGUILayout.EndHorizontal();

                DrawUsages(FactsSearch.SelectedItem);
            }

            //Draw Event
            DrawEntryInfo(EventsSearch.SelectedItem);
            if (EventsSearch.SelectedItem != null)
            {
                var selectedEvent = EventsSearch.SelectedItem;

                EditorGUILayout.BeginHorizontal("box");
                if (selectedEvent.Rule != null)
                    EditorGUILayout.LabelField($"Next Rule: {selectedEvent.Rule.Name}");
                else
                    EditorGUILayout.LabelField($"Select Rule", EditorStyles.boldLabel);

                if (GUILayout.Button("+", GUI.skin.label, GUILayout.MaxWidth(30f)))
                {
                    var window = EditorExtentions.CreateSearchWindowBaseEntry(typeof(RuleEntry));
                    window.SetValues(SelectedTable.Rules, (selection) => { selectedEvent.Rule = selection as RuleEntry; });
                }

                if (GUILayout.Button("-", GUI.skin.label, GUILayout.MaxWidth(30f)))
                {
                    selectedEvent.Rule = null;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal("box");
                
                var options = Enum.GetNames(typeof(EventEntryType));
                _selected = (int)selectedEvent.Type;
                _selected = EditorGUILayout.Popup("Type", _selected, options);
                selectedEvent.Type = (EventEntryType)Enum.Parse(typeof(EventEntryType), options[_selected]);

                //TODO: Remove hardcoded type of object field
                switch (selectedEvent.Type)
                {
                    case EventEntryType.None:
                        break;
                    case EventEntryType.Dialogue:
                        selectedEvent.Object = EditorGUILayout.ObjectField((UnityEngine.Object)selectedEvent.Object, typeof(DialogueContainerRuntimeData), true);
                        break;
                    case EventEntryType.Quest:
                        selectedEvent.Object = EditorGUILayout.ObjectField((UnityEngine.Object)selectedEvent.Object, typeof(QuestData), true);
                        break;
                    case EventEntryType.RandomDialogue:
                        selectedEvent.Object = EditorGUILayout.ObjectField((UnityEngine.Object)selectedEvent.Object, typeof(DialogueRandomData), true);
                        break;
                    default:
                        break;
                }

                EditorGUILayout.EndHorizontal();

                DrawUsages(EventsSearch.SelectedItem);

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField($"Once: ", GUILayout.MaxWidth(40f));
                EventsSearch.SelectedItem.Once = EditorGUILayout.Toggle(EventsSearch.SelectedItem.Once);
                EditorGUILayout.EndHorizontal();
            }
            DrawLogicalEntry(EventsSearch.SelectedItem);

            //Draw Rule
            DrawEntryInfo(RulesSearch.SelectedItem);
            if (RulesSearch.SelectedItem != null)
            {
                RuleEventSearch.OnGUI = DrawEventSearchWindow;

                EditorGUILayout.BeginVertical("box");

                EditorExtentions.DrawFoldoutGroup("Triggers");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("");
                RuleEventSearch.DrawButtons(RulesSearch.SelectedItem.Triggers);
                EditorGUILayout.EndHorizontal();

                RuleEventSearch.DrawElements(RulesSearch.SelectedItem.Triggers);

                EditorGUILayout.EndVertical();
                DrawUsages(RulesSearch.SelectedItem);
            }

            DrawLogicalEntry(RulesSearch.SelectedItem);

            EditorGUILayout.EndVertical();
        }
        #endregion

        public void DrawEventSearchWindow(int index)
        {
            if (GUILayout.Button("+", GUI.skin.label, GUILayout.Width(30f)))
            {
                var searchWindow = EditorExtentions.CreateSearchWindowBaseEntry(typeof(EventEntry));
                searchWindow.SetValues(SelectedTable.Events, (selection) => { RulesSearch.SelectedItem.Triggers[index] = selection as EventEntry; });
            }
        }

        //This methods is used by only BaseEntry datas
        #region General Draw Methods
        private void DrawEntryInfo(BaseEntry baseEntry)
        {
            if (baseEntry == null) return;

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("Name: ", GUILayout.Width(50f));
            baseEntry.Name = EditorGUILayout.TextField(baseEntry.Name);
            EditorGUILayout.EndHorizontal();
        }

        public void DrawLogicalEntry(LogicalEntry logicalEntry)
        {
            if (logicalEntry == null) return;
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Criterias"))
            {
                _isCriterias = true;
                _isModifiers = false;
            }

            if (GUILayout.Button("Modifiers"))
            {
                _isCriterias = false;
                _isModifiers = true;
            }
            EditorGUILayout.EndHorizontal();

            if (_isCriterias)
                DrawCriterias(logicalEntry.Criterias);
            if (_isModifiers)
                DrawModifiers(logicalEntry.Modifications);
            EditorGUILayout.EndVertical();
        }

        //Draw crierias for EventEntry/RuleEntry
        private void DrawCriterias(List<Criteria> criterias)
        {
            CriteriaDrawer.SetValues(criterias, SelectedTable.Facts);
            CriteriaDrawer.OnGUI();
        }

        //Draw modifiers for EventEntry/RuleEntry
        public void DrawModifiers(List<Modifier> modifications)
        {
            ModifiersDrawer.SetValues(modifications, SelectedTable.Facts);
            ModifiersDrawer.OnGUI();
        }

        public void DrawUsages(BaseEntry baseEntry)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField($"Usage: {baseEntry.Usages}");
            if (GUILayout.Button("+", GUI.skin.label, GUILayout.Width(30f))) baseEntry.Usages++;
            if (GUILayout.Button("-", GUI.skin.label, GUILayout.Width(30f))) baseEntry.Usages--;
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}