using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private GameObject _poolObject;
    private Transform _rootPool;
    private string _poolName;
    private string _poolObjectName;

    private Action _onPoolRemove;

    private readonly Dictionary<string, HashSet<T>> _pool;
    private readonly int _capacityPool;

    public Pool(AssetsContext assetsContext, string poolName, string poolObjectName, int capacityPool)
    {
        _poolObjectName = poolObjectName;
        _poolName = poolName;
        _poolObject = (GameObject)assetsContext.GetObjectOfType(typeof(GameObject), _poolObjectName);
        _pool = new Dictionary<string, HashSet<T>>();
        _capacityPool = capacityPool;

        InitializePool();
    }

    public bool TryGetPoolObject(out T poolObject)
    {
        poolObject = null;
        
        TryGetPoolObject(GetListElements(_poolObjectName), out T result);
        if (result == null) return false;
        
        poolObject = result;
        return true;
    }

    private HashSet<T> GetListElements(string type)
    {
        return _pool.ContainsKey(type) ? _pool[type] : _pool[type] = new HashSet<T>();
    }

    private bool TryGetPoolObject(HashSet<T> poolObjects, out T poolObject)
    {
        poolObject = poolObjects.FirstOrDefault(a => !a.gameObject.activeSelf);        
        if (poolObject == null) return false;

        var component = poolObject.gameObject.transform;
        _onPoolRemove += () => ReturnToPool(component.transform);
        
        return true;
    }

    private void ReturnToPool(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.gameObject.SetActive(false);
        transform.SetParent(_rootPool);
    }

    public void InitializePool()
    {
        if (!_rootPool)
        {
            _rootPool = new GameObject(_poolName).transform;
            UnityEngine.Object.DontDestroyOnLoad(_rootPool);

            if (!_pool.ContainsKey(_poolObjectName)) _pool[_poolObjectName] = new HashSet<T>();

            for (var i = 0; i < _capacityPool; i++)
            {
                var instantiate = UnityEngine.Object.Instantiate(_poolObject);
                ReturnToPool(instantiate.transform);
                _pool[_poolObjectName].Add(instantiate.GetComponent<T>());
            }
        }
    }

    public void RefreshPool() => _onPoolRemove?.Invoke();

    public void RemovePool()
    {
        UnityEngine.Object.Destroy(_rootPool.gameObject);
    }
}