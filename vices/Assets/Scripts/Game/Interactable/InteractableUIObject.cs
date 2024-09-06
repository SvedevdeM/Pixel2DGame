using UnityEngine;

namespace Vices.Scripts
{
    public class InteractableUIObject : PoolObject
    {
        private void Start()
        {
            _poolName = "InteractableUI Pool";
        }

        protected override void OnActive(Vector3 position, Quaternion rotation) { }
        protected override void OnReturnToPool() { }
    }
}