using UnityEditor;
using JabberwockyWorld.AnimationSystem.Scripts;
#if UNITY_EDITOR
namespace JabberwockyWorld.AnimationSystem.Editor
{
    [CustomEditor(typeof(SwitchNode))]
    public class SwitchNodeEditor : AnimatorNodeEditor
    {
        protected void OnEnable()
        {
            AddCustomProperty("controlDriver");
            AddCustomProperty("drivers");
            AddCustomProperty("nodes");
        }
    }
}
#endif