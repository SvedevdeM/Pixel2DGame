using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [CreateAssetMenu(fileName = "Drivers", menuName = "Animation System/Drivers", order = 400)]
    public class Drivers : ScriptableObject
    {
        public List<string> Names = new List<string>();
    }
}