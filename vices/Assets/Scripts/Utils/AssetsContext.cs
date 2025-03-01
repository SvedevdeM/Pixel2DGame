﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = nameof(AssetsContext), menuName = "JabberwockyWorld/" + nameof(AssetsContext), order = 0)]
public class AssetsContext : ScriptableObject
{
    [SerializeField] private Object[] _objects;

    public int Count => _objects.Length;

    public Object GetObjectOfType(Type targetType, string targetName = null)
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            var obj = _objects[i];
            if (obj.GetType().IsAssignableFrom(targetType))
            {
                if (targetName == null || obj.name == targetName)
                {
                    return obj;
                }
            }
        }
        return null;
    }
}