using System;
using System.Collections;
using System.Collections.Generic;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    public interface IReadOnlyAnimatorState : IEnumerable<KeyValuePair<string, int>>
    {
        void Set(string name, bool value);

        int Get(string name, int fallback = 0);

        bool GetBool(string name, bool fallback = false);

        bool ShouldFlip();
    }

    [Serializable]
    public class AnimatorState : IReadOnlyAnimatorState
    {
        public static readonly string FlipDriver = "_flip";


        private readonly Dictionary<string, int> _drivers = new Dictionary<string, int>();

        public void Set(string name, int value)
        {
            _drivers[name] = value;
        }


        public void Set(string name, bool value)
        {
            _drivers[name] = value ? 1 : 0;
        }

        public int Get(string name, int fallback = 0)
        {
            return _drivers.ContainsKey(name) ? _drivers[name] : fallback;
        }

        public bool GetBool(string name, bool fallback = false)
        {
            return _drivers.ContainsKey(name) ? _drivers[name] != 0 : fallback;
        }

        public bool Contains(string name)
        {
            return _drivers.ContainsKey(name);
        }

        public void Remove(string name)
        {
            _drivers.Remove(name);
        }

        public bool ShouldFlip()
        {
            return GetBool(FlipDriver);
        }

        public void Clear()
        {
            _drivers.Clear();
        }

        public void Merge(AnimatorState state)
        {
            foreach (var driver in state)
                _drivers[driver.Key] = driver.Value;
        }

        public void Merge(DriverDictionary drivers)
        {
            for (var i = 0; i < drivers.keys.Count; i++)
                if ( drivers.keys[i] != null)
                    if (drivers.keys[i].Name != null )
                        _drivers[drivers.keys[i].Name] = drivers.values[i];
        }

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return _drivers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}