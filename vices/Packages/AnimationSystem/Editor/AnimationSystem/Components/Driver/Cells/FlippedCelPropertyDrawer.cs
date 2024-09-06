using JabberwockyWorld.AnimationSystem.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor
{
    [CustomPropertyDrawer(typeof(FlippedCel))]
    public class FLippedCelPropertyDrawer : CelPropertyDrawer
    {
        protected override IEnumerable<string> PropertiesToDraw => new[] { "sprite", "spriteLeft", "drivers" };

        protected override Rect DrawPreview(Rect position, SerializedProperty property)
        {
            position = DrawSpritePreview(position, property, "sprite");
            position = DrawSpritePreview(position, property, "spriteLeft");

            return position;
        }
    }
}
#endif