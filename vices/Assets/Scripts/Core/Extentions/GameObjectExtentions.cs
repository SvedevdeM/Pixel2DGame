using UnityEngine;

namespace Vices.Scripts.Core
{
    public static class GameObjectExtentions
    {
        public static T GetComponentInObject<T>(this GameObject gameObject) where T : Component
        {
            T result = null;
            if (!gameObject) return null;
            if (gameObject.GetComponent<T>())
            {
                result = gameObject.GetComponent<T>();
                return result;
            }

            if (gameObject.GetComponentInChildren<T>())
            {
                result = gameObject.GetComponentInChildren<T>();
                return result;
            }

            if (gameObject.GetComponentInParent<T>())
            {
                result = gameObject.GetComponentInParent<T>();
                return result;
            }

            return null;
        }

        public static T GetComponentInObject<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.gameObject.GetComponentInObject<T>();
        }

        public static bool TryGetComponentInObject<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = GetComponentInObject<T>(gameObject);

            if (component == null || component == default) return false;
            return true;
        }
    }
}