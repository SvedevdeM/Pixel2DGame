using System.Collections.Generic;
using UnityEditor;
using JabberwockyWorld.AnimationSystem.Scripts;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor
{
    [CustomPropertyDrawer(typeof(CommonCel))]
    public class SimpleCelPropertyDrawer : CelPropertyDrawer
    {
        protected override IEnumerable<string> PropertiesToDraw => new[] {"sprite", "drivers"};
    }
}
#endif