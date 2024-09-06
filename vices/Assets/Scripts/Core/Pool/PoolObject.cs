using UnityEngine;

namespace Vices.Scripts
{
    public abstract class PoolObject : MonoBehaviour, IPoolObject
    {
        protected string _poolName;
        private Transform _rootPool;
        public Transform RootPool
        {
            get
            {
                if (_rootPool) return _rootPool;

                var find = GameObject.Find(_poolName);
                _rootPool = find == null ? null : find.transform;
                return _rootPool;
            }
        }

        protected abstract void OnActive(Vector3 position, Quaternion rotation);
        protected abstract void OnReturnToPool();

        public void Active(Vector3 position, Quaternion rotation)
        {
            transform.localPosition = position;
            transform.localRotation = rotation;
            gameObject.SetActive(true);
            transform.SetParent(null);

            OnActive(position, rotation);
        }

        public void ReturnToPool()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            gameObject.SetActive(false);
            transform.SetParent(RootPool);

            OnReturnToPool();

            if (!RootPool) Destroy(gameObject);
        }
    }
}