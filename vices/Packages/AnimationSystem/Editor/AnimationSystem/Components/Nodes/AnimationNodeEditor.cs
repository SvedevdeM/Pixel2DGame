using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor 
{

    public class AnimationNodeEditor : AnimatorNodeEditor
    {
        protected static float FPS = 10;
        protected static int CurrentFrame;
        protected static bool ShouldPlay = true;
        protected readonly List<Sprite> Sprites = new List<Sprite>();
        protected SerializedProperty Cels;


        protected virtual void OnEnable()
        {
            //AddCustomProperty("controlDriverNew");
            AddCustomProperty("drivers");
            Cels = AddCustomProperty("cels");
        }

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawCustomProperties();
            EditorGUILayout.Separator();
            UpdateSpritesCache();

            serializedObject.ApplyModifiedProperties();
        }


        protected virtual void UpdateSpritesCache()
        {
            Sprites.Clear();
            for (var i = 0; i < Cels.arraySize; i++) 
            {
                var frameProp = Cels.GetArrayElementAtIndex(i);
                var sprite = frameProp.FindPropertyRelative("sprite").objectReferenceValue as Sprite;
                if (sprite != null)
                    Sprites.Add(sprite);
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return FPS > 0 && ShouldPlay;
        }

        public override void OnPreviewGUI(Rect position, GUIStyle background)
        {
            DrawPlaybackControls();

            if (Sprites.Count == 0) return;
            int index = ShouldPlay
                ? (int)(EditorApplication.timeSinceStartup * FPS % Sprites.Count)
                : CurrentFrame;

            DrawUtility.DrawTexturePreview(position, Sprites[index]);
        }

        protected void DrawPlaybackControls()
        {
            EditorGUILayout.BeginHorizontal();
            ShouldPlay = EditorGUILayout.ToggleLeft("Play", ShouldPlay, GUILayout.MaxWidth(100));
            if (ShouldPlay)
                FPS = EditorGUILayout.FloatField("Frames per seconds", FPS);
            else
                CurrentFrame = EditorGUILayout.IntSlider(CurrentFrame, 0, Sprites.Count - 1);
            EditorGUILayout.EndHorizontal();
        }

    }
}
#endif