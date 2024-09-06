using System;
using System.Collections.Generic;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [Serializable]
    public class DriverDictionary
    {
        //[SerializeField] public List<string> keys = new List<string>();
        [SerializeField] public List<int> values = new List<int>();
        [SerializeField] public List<Controll> keys = new List<Controll>();
    }
}