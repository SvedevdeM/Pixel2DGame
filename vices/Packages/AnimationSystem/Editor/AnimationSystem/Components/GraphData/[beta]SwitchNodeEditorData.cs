using JabberwockyWorld.AnimationSystem.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    [Serializable]
    public class SwitchNodeEditorData
    {
        [SerializeField] public string ID;
        [SerializeField] public string Name;
        [SerializeField] public Drivers Drivers;
        [SerializeField] public List<AnimationChoiceEditorData> Choices;
        [SerializeField] public Controll Controll;
        [SerializeField] public string GroupID;
        [SerializeField] public Animation Type;//?
        [SerializeField] public Vector2 Position;
    }
}
