#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace JabberwockyWorld.AnimationSystem.Editor.Utiilites
{
    public static class AnimationStyleUtility
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
#endif
