using UnityEngine;
using UnityEngine.Pool;

public class FactoryPool<T> where T : MonoBehaviour
{
    private ObjectPool<T> _pool;

    public void Init(T prefab, int startSize = 10)
    {
        if (!prefab)
            return;

        _pool = new(
            createFunc: () => Object.Instantiate(prefab),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Object.Destroy(obj),
            collectionCheck: true,
            defaultCapacity: startSize
            );
    }

    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T obj)
    { 
        _pool.Release(obj); 
    }
}