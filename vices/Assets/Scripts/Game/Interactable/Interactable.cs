using UnityEngine;
using Vices.Scripts.Core;

namespace Vices.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class Interactable : MonoBehaviour, IExecutable
    {
        private InteractablePool _pool;
        private InteractableUIObject _interactableUI;
        protected bool _canInteract;
        protected bool _isNeedUi = true;

        protected abstract void OnEnter(Collider other);
        protected abstract void OnInteract();

        protected virtual void Start()
        {
            _pool = GameContext.Context.InteractablePool;
            GameManager.Singleton.AddExecutable(this);            
        }

        public void OnTriggerEnter(Collider other)
        {
            OnEnter(other);

            _canInteract = true;

            if (!_isNeedUi) return;
            if (!_pool.TryGetPoolObject(out _interactableUI)) Debug.LogWarning("Pool object can't be taken from pool. Please check pool settings");
            if (_interactableUI == null) return;
            _interactableUI.Active(transform.position, Quaternion.identity);
        }

        public void Execute()
        {
            if (_canInteract && Input.GetKeyDown(KeyCode.E)) //TODO: Change Input to new input system
            {
                OnInteract();
                if (_isNeedUi) _interactableUI.ReturnToPool();
                _canInteract = false;
            }
        }

        public void OnTriggerExit(Collider other)
        {
           _canInteract = false;

            if (!_isNeedUi) return;
            _interactableUI.ReturnToPool();
        }

        private void OnDestroy()
        {
            GameManager.Singleton.RemoveExecutable(this);
        }
    }
}