using UnityEngine;
using Vices.Scripts.Core;

namespace Vices.Scripts.Game
{
    public class DynamicSpriteMask : Executable
    {
        [SerializeField] private SpriteMask[] _masks;

        private Transform _playerTransform;
        private bool _enabled = false;
        private bool _disabled = false;

        public override void Start()
        {
            //TODO: Change getting player transform from game context
            _playerTransform = FindObjectOfType<PlayerMovement>().transform;
            base.Start();
        }

        public override void Execute()
        {
            if (_playerTransform == null) return;
            if (!_enabled && _playerTransform.position.y > gameObject.transform.position.y)
            {
                ChangeMaskState(true);
                _enabled = true;
                _disabled = false;
            }
            else if (!_disabled && _playerTransform.position.y <= gameObject.transform.position.y)
            {
                ChangeMaskState(false);
                _enabled = false;
                _disabled = true;
            }
        }

        private void ChangeMaskState(bool state)
        {
            for (int i = 0; i < _masks.Length; i++)
            {
                _masks[i].enabled = state;
            }
        }
    }
}