using System.Collections.Generic;
using UnityEngine;

//the drivet issue
//+ temprorary drivers in specific
//listeners
namespace JabberwockyWorld.AnimationSystem.Scripts
{
    public delegate void AnimatorListener();
    public class SpriteAnimator : MonoBehaviour
    {
        public event AnimatorListener Ticked;

        /// <summary>
        /// SpriteRenderer is mean for displaying the animation.
        /// </summary>
        public SpriteRenderer Renderer => renderer;
        /// <summary>
        /// Timer inside class, used for sprites per second.
        /// </summary>
        private float _clock = 0.0f;
        /// <summary>
        /// Frames per second.
        /// </summary>
        [SerializeField]
        private int fps = 12;
        [Tooltip("Connection between objects and animations.")]
        public AnimatorNode Root;

        [Tooltip("SpriteRenderer being used to display the animation.")]
        [SerializeField]
        private new SpriteRenderer renderer;

        private readonly AnimatorState _previousState = new AnimatorState();
        private readonly AnimatorState _nextState = new AnimatorState();
        /// </summary>
        public IReadOnlyAnimatorState State => _previousState;
        public IReadOnlyAnimatorState NextState => _nextState;

        /// <summary>
        /// Shortcut for setting the <see cref="AnimationState.FlipDriver"/>
        /// </summary>
        public bool Flip
        {
            set => _previousState.Set(AnimatorState.FlipDriver, value);
        }

        private readonly Dictionary<string, AnimatorListener> _listeners =
            new Dictionary<string, AnimatorListener>();

        private void Awake()
        {
            if (renderer == null)
                renderer = GetComponent<SpriteRenderer>();
            //finding root in graph
        }

        private void Update()
        {
            _clock += Time.deltaTime;
            float secondsPerFrame = 1 / (float)fps;
            while (_clock >= secondsPerFrame)
            {
                _clock -= secondsPerFrame;
                UpdateFrame();
            }
        }

        private void UpdateFrame()
        {
            _previousState.Merge(_nextState);
            _nextState.Clear();
            Root
                 .Resolve(_previousState, _nextState)
                 .ResolveCel(_previousState, _nextState)
                 .ApplyToRenderer(_previousState, _nextState, renderer);
            
            Ticked?.Invoke();
        }

        public void Set(string key, int value)
        {
            _nextState.Set(key, value);
        }
        public void Set(string key, bool value)
        {
            _nextState.Set(key, value);
        }
        public bool WillChange(string key)
        {
            return _nextState.Contains(key) && _nextState.Get(key) != _previousState.Get(key);
        }
        public bool WillChange(string key, int toValue)
        {
            if (!_nextState.Contains(key)) return false;
            int nextValue = _nextState.Get(key);

            return nextValue == toValue && _previousState.Get(key) != nextValue;
        }

        public bool WillChange(string key, bool toValue)
        {
            if (!_nextState.Contains(key)) return false;
            bool nextValue = _nextState.GetBool(key);

            return nextValue == toValue && _previousState.GetBool(key) != nextValue;
        }

        /// <summary>
        /// Immediately force the next resolution process to happen.
        /// </summary>
        public void ForceRerender()
        {
            _clock = 0;
            UpdateFrame();
        }

        public void AddListener(string driverName, AnimatorListener listener)
        {
            if (_listeners.ContainsKey(driverName))
                _listeners[driverName] += listener;
            else
                _listeners[driverName] = listener;
        }

        /// <summary>
        /// Remove listener for the given driver.
        /// </summary>
        /// <param name="driverName"></param>
        /// <param name="listener"></param>
        public void RemoveListener(string driverName, AnimatorListener listener)
        {
            if (!_listeners.ContainsKey(driverName)) return;

            _listeners[driverName] -= listener;
            if (_listeners[driverName] == null)
                _listeners.Remove(driverName);
        }

    }

}
