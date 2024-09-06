using JabberwockyWorld.AnimationSystem.Scripts;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor
{
    [CustomPropertyDrawer(typeof(ControlDriverNew))]
    public class ControlDriverPropertyDrawer : PropertyDrawer
    {
        private const float Spacing = 1;
        private const float Margin = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //float name = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("name"));
            //float ai = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("autoIncrement"));
            float control = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("controll"));
            //float percentage = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("percentageBased"));


            return control + Spacing * 2 + Margin * 2;
            /*return property.isExpanded
                ? control + Spacing * 200 + Margin * 2
                :  control + Spacing * 2 + Margin * 2;*/
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            //var name = property.FindPropertyRelative("name");
            //var ai = property.FindPropertyRelative("autoIncrement");
            var control = property.FindPropertyRelative("controll");
            //var percentage = property.FindPropertyRelative("percentageBased");

            //var labelText = "Controller";
            //if ((control.) != null)
             //   labelText += $" ({control.stringValue})";

            position.y += Margin;
            position.y += Spacing;
            position.y += Spacing;
            EditorGUI.PropertyField(position, control);
            position.y += position.height + Spacing;
            //position.height = EditorGUI.GetPropertyHeight(name); //+ EditorGUI.GetPropertyHeight(control);
            //property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, labelText, true);
            position.y += position.height + Spacing;
            /*
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                //position.y += Spacing;
                //EditorGUI.PropertyField(position, name);
                //position.y += position.height + Spacing;
                position.y += Spacing;
                EditorGUI.PropertyField (position, control);
                position.y += position.height + Spacing;

                //EditorGUI.BeginDisabledGroup(percentage.boolValue);
                //position.y += Spacing;
                //position.height = EditorGUI.GetPropertyHeight(ai);
                //EditorGUI.PropertyField(position, ai);
                //position.y += position.height + Spacing;
                EditorGUI.EndDisabledGroup();

                //EditorGUI.BeginDisabledGroup(ai.boolValue);
                //position.y += Spacing;
                //position.height = EditorGUI.GetPropertyHeight(percentage);
               // EditorGUI.PropertyField(position, percentage);
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }*/

            EditorGUI.EndProperty();
        }
    }
}
#endif