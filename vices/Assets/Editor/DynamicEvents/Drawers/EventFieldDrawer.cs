using UnityEngine;
using UnityEditor;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Structs;
using JabberwockyWorld.DynamicEvents.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Editor.Utilites;


namespace JabberwockyWorld.DynamicEvents.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(EventField))]
    public class EventFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var eventProperty = property.FindPropertyRelative("EventEntry");
            var name = eventProperty.FindPropertyRelative("Name").stringValue;

            position.width -= 24f;
            EditorGUI.LabelField(position, $"Event: {name}");
            position.x += position.width;
            if (GUI.Button(position, "+", GUI.skin.label))
            {
                var database = Resources.FindObjectsOfTypeAll<EventDatabase>();
                var eventTablesWindow = EditorExtentions.CreateSearchWindow(typeof(EventTable));
                eventTablesWindow.SetValues(database[0].EventTables, (s) => {
                    var window = EditorExtentions.CreateSearchWindowBaseEntry(typeof(EventEntry));
                    window.SetValues(s.Events, (selected) =>
                    {
                        eventProperty.FindPropertyRelative("Name").stringValue = (selected as EventEntry).Name;
                        eventProperty.FindPropertyRelative("ID").stringValue = (selected as EventEntry).ID;
                        eventProperty.serializedObject.ApplyModifiedProperties();
                        window.Close();
                    });
                });
            }
        }       
    }
}