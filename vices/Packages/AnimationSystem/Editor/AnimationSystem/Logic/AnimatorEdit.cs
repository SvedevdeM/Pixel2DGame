using JabberwockyWorld.AnimationSystem.Scripts;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor
{

    [CustomEditor(typeof(SpriteAnimator))]
    public class AnimatorEdit : UnityEditor.Editor
    {
        private SerializedProperty _root;
        private SerializedProperty _renderer;
        private SerializedProperty _fps;
        //private SerializedProperty _temporary;
        private SpriteAnimator _animator;
        private bool _isExpanded;

        private void OnEnable()
        {
            _root = serializedObject.FindProperty(nameof(SpriteAnimator.Root));
            _renderer = serializedObject.FindProperty("renderer");
            _fps = serializedObject.FindProperty("fps");
            //_temporary = serializedObject.FindProperty("temporaryDrivers");
            _animator = target as SpriteAnimator;
        }

        public override bool RequiresConstantRepaint()
        {
            return _isExpanded && _animator != null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_root);
            EditorGUILayout.PropertyField(_renderer);
            EditorGUILayout.PropertyField(_fps);
            //EditorGUILayout.PropertyField(_temporary);

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            _isExpanded = EditorGUILayout.Foldout(_isExpanded, "State", true);
            EditorGUI.EndDisabledGroup();

            if (Application.isPlaying && _isExpanded && _animator != null)
            {
                var boxStyle = new GUIStyle("Box")
                {
                    padding = new RectOffset(8, 8, 8, 8),
                    margin = new RectOffset(8, 8, 8, 8)
                };

                EditorGUIUtility.labelWidth /= 2;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical(boxStyle);
                EditorGUILayout.LabelField("Previous", EditorStyles.boldLabel);
                EditorGUILayout.Separator();
                foreach (var pair in _animator.State)
                    EditorGUILayout.IntField(pair.Key, pair.Value);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(boxStyle);
                EditorGUILayout.LabelField("Next", EditorStyles.boldLabel);
                EditorGUILayout.Separator();
                foreach (var pair in _animator.NextState)
                    EditorGUILayout.IntField(pair.Key, pair.Value);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = 0;
            }
            else
            {
                _isExpanded = false;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif