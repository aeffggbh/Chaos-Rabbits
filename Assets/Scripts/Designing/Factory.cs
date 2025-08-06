using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;

public class Factory<TClass> where TClass : class
{
    private IObjectPool<TClass> pool;

    public static TPrefab Create<TPrefab>(TPrefab prefab, Transform parent = null) where TPrefab : Object
    {
        return parent ?
            Object.Instantiate(prefab, parent) :
            Object.Instantiate(prefab);
    }
}
