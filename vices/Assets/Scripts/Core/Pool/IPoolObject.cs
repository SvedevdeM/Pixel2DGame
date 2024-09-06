using UnityEngine;

namespace Vices.Scripts
{
    public interface IPoolObject
    {
        void Active(Vector3 position, Quaternion rotation);
        void ReturnToPool();
    }
}