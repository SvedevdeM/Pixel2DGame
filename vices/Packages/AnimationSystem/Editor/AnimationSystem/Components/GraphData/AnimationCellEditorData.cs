using JabberwockyWorld.AnimationSystem.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Editor
{
    [Serializable]
    public class AnimationCellEditorData
    {
        [SerializeField] public Sprite Sprite;
        [SerializeField] public List<AnimationDriverDIctEditorData> DriverDIctEditorDatas;
    }
}
