using UnityEditor;
using UnityEngine;

namespace Vices.Scripts.Core
{
    [CreateAssetMenu(fileName = nameof(SceneData), menuName = "JabberwockyWorld/Scenes/" + nameof(SceneData))]
    public class SceneData : ScriptableObject
    {
        public string Name;
        public SceneAsset[] Scenes;
    }
}