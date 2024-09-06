using JabberwockyWorld.AnimationSystem.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    [Serializable]
    public class AnimationNodeEditorData
    {
        [SerializeField] public string ID;
        [SerializeField] public string Name;
        [SerializeField] public Controll Controll;
        [SerializeField] public List<AnimationDriverDIctEditorData> Driversofnode;
        [SerializeField] public string GroupID;
        [SerializeField] public List<AnimationCellEditorData> Cells;
        [SerializeField] public List<AnimationChoiceEditorData> Choices;
        //[SerializeField] public Sprite[] cells;
        //[SerializeField] public Drivers DriversForCells;
        [SerializeField] public AnimationType Type;//!?
        [SerializeField] public Vector2 Position;
    }
}
