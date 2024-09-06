using System;
using UnityEngine;

namespace Vices.Scripts.Game
{
    [Serializable]
    public class CutsceneTimeline
    {
        [SerializeField] private CutsceneKey[] _keys;
        public CutsceneKey[] Keys => _keys;
    }
}