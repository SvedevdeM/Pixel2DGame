using UnityEngine;
using UnityEditor;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Structs;
using JabberwockyWorld.DynamicEvents.Scripts.Data;
using JabberwockyWorld.DynamicEvents.Editor.Utilites;


namespace JabberwockyWorld.DynamicEvents.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(RuleField))]
    public class RuleFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var ruleProperty = property.FindPropertyRelative("RuleEntry");
            var name = ruleProperty.FindPropertyRelative("Name").stringValue;

            position.width -= 24f;
            EditorGUI.LabelField(position, $"Rule: {name}");
            position.x += position.width;
            if (GUI.Button(position, "+", GUI.skin.label))
            {
                var database = Resources.FindObjectsOfTypeAll<EventDatabase>();
                var eventTablesWindow = EditorExtentions.CreateSearchWindow(typeof(EventTable));
                eventTablesWindow.SetValues(database[0].EventTables, (s) => {
                    var window = EditorExtentions.CreateSearchWindowBaseEntry(typeof(RuleEntry));
                    window.SetValues(s.Rules, (selected) =>
                    {
                        ruleProperty.FindPropertyRelative("Name").stringValue = (selected as RuleEntry).Name;
                        ruleProperty.FindPropertyRelative("ID").stringValue = (selected as RuleEntry).ID;
                        ruleProperty.serializedObject.ApplyModifiedProperties();
                        window.Close();
                    });
                });
            }
        }
    }
}