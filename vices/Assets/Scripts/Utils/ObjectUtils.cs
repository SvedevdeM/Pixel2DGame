using UnityEngine;

namespace Vices.Scripts.Core
{
    public static class ObjectUtils
    {
        public static T CreateGameObject<T>(GameObject prefab) where T : Component
        {
            return UnityEngine.Object.Instantiate(prefab).GetComponentInObject<T>();
        }

        public static T CreateGameObject<T>(GameObject prefab, Transform parent) where T : Component
        {
            return UnityEngine.Object.Instantiate(prefab, parent).GetComponentInObject<T>();
        }

        public static T CreateGameObject<T>(GameObject prefab, Vector3 position) where T : Component
        {
            return UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, null).GetComponentInObject<T>();
        }
    }
}