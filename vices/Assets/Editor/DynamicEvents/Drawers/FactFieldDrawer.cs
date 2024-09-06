using UnityEngine;
using UnityEditor;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Structs;
using JabberwockyWorld.DynamicEvents.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Editor.Utilites;


namespace JabberwockyWorld.DynamicEvents.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(FactField))]
    public class FactFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var factProperty = property.FindPropertyRelative("FactEntry");
            var name = factProperty.FindPropertyRelative("Name").stringValue;

            position.width -= 24f;
            EditorGUI.LabelField(position, $"Fact: {name}");
            position.x += position.width;
            if (GUI.Button(position, "+", GUI.skin.label))
            {
                var database = Resources.FindObjectsOfTypeAll<EventDatabase>();
                var eventTablesWindow = EditorExtentions.CreateSearchWindow(typeof(EventTable));
                eventTablesWindow.SetValues(database[0].EventTables, (s) => {
                    var window = EditorExtentions.CreateSearchWindowBaseEntry(typeof(FactEntry));
                    window.SetValues(s.Facts, (selected) =>
                    {
                        factProperty.FindPropertyRelative("Name").stringValue = (selected as FactEntry).Name;
                        factProperty.FindPropertyRelative("ID").stringValue = (selected as FactEntry).ID;
                        factProperty.serializedObject.ApplyModifiedProperties();
                        window.Close();
                    });
                });
            }
        }
    }
}