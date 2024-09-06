using System.Collections.Generic;
using UnityEngine;

namespace Vices.Scripts
{
    //This script is used for managing logic of disabling cutscenes
    //whose didn't have criteria checking logic
    [CreateAssetMenu(fileName = nameof(CutscenesInfo), menuName = "Vices/" + nameof(CutscenesInfo))]
    public class CutscenesInfo : ScriptableObject
    {
        private Dictionary<string, bool> _cutscenesInfo;

        public void Initialize()
        {
            _cutscenesInfo = new Dictionary<string, bool>();
            _cutscenesInfo.Clear();
        }

        public bool CheckSetting(string cutsceneName)
        {
            if (!_cutscenesInfo.TryGetValue(cutsceneName, out bool value))
            {
                _cutscenesInfo.Add(cutsceneName, false);
                return false;
            }

            return value;
        }

        public void SetSettting(string cutsceneName, bool setting)
        {
            if(!_cutscenesInfo.TryGetValue(cutsceneName, out bool value))
            {
                _cutscenesInfo.Add(cutsceneName, setting);
                return;
            }

            _cutscenesInfo.Remove(cutsceneName);
            _cutscenesInfo.Add(cutsceneName, setting);
        }
    }
}