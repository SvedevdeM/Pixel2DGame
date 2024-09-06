using UnityEditor;
using UnityEngine.UIElements;

namespace JabberwockyWorld.DialogueSystem.Editor.Utilities
{
    public static class DialogueStyleUtility
    {
        public static VisualElement AddClasses(this VisualElement element, params string[] classes)
        {
            foreach (var @class in classes)
            {
                element.AddToClassList(@class);
            }

            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styles) 
        {
            foreach (var style in styles)
            {
                StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load(style);

                element.styleSheets.Add(styleSheet);
            }

            return element;
        }
    }
}
