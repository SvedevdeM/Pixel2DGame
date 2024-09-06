using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [Serializable]
    public class AnimationCellsData
    {
        public Sprite Sprite;
        public List<AnimationDriverDIctData> DriverDIctDatas = new List<AnimationDriverDIctData>();
    }
}

