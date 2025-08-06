using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "EnemyFactory", menuName = "ScriptableObjects/Factory/EnemyFactory")]
public class EnemyFactory : ScriptableObject
{
    [SerializeField] private List<EnemyData> _enemyData;
    private Dictionary<GameObject, ObjectPool<Enemy>> _enemyPools;

    private void OnEnable()
    {
        _enemyPools = new();

        _enemyData.RemoveAll(data => data == null || data.Prefab == null);

        foreach (var data in _enemyData)
        {
            if (data != null && !_enemyPools.ContainsKey(data.Prefab))
            {
                _enemyPools[data.Prefab] = new(
                    CreateEnemy(data),
                    (enemy) => OnGet(enemy, data.Stats, data.Prefab),
                    (enemy) => enemy.gameObject.SetActive(false),
                    (enemy) => enemy.gameObject.SetActive(false),
                    true,
                    10,
                    20
                    );
            }
        }
    }

    private void OnGet(Enemy enemy, EnemyStats stats, GameObject prefab)
    {
        if (enemy == null || enemy.gameObject == null)
            return;

        enemy.gameObject.SetActive(true);
        enemy.Stats = stats;
        enemy.PrefabSource = prefab;
        enemy.Reset();

        enemy.SetPoolReleaseAction(() =>
        {
            if (_enemyPools.TryGetValue(enemy.PrefabSource, out var pool))
            {
                pool.Release(enemy);
            }
            else
                Debug.LogWarning("No pool found for enemy " + enemy.gameObject.name);
        });
    }

    private Func<Enemy> CreateEnemy(EnemyData data)
    {
        return () =>
        {
            var instance = Instantiate(data.Prefab);
            var enemy = instance.GetComponent<Enemy>();
            return enemy;
        };
    }

    private Action ReleaseEnemy(GameObject prefab, Enemy enemy)
    {
        return () =>
        {
            if (enemy == null || enemy.gameObject == null)
                return;

            if (_enemyPools.TryGetValue(prefab, out var pool))
                pool.Release(enemy);
        };
    }

    public Enemy CreatePoolEnemy(EnemyData data)
    {
        var instance = Instantiate(data.Prefab);
        var enemy = instance.GetComponent<Enemy>();
        enemy.SetPoolReleaseAction(ReleaseEnemy(data.Prefab, enemy));
        return enemy;
    }

    public Enemy CreateEnemy(Transform parent)
    {
        var data = GetRandomData();

        if (!_enemyPools.TryGetValue(data.Prefab, out var pool))
            return null;

        var enemy = pool.Get();
        enemy.transform.SetParent(parent);
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
        enemy.transform.localScale = Vector3.one;

        return enemy;
    }

    private EnemyData GetRandomData()
    {
        if (_enemyData.Count == 0)
            return null;

        var index = UnityEngine.Random.Range(0, _enemyData.Count);
        var data = _enemyData[index];

        return data;
    }

    private void Reset()
    {
        foreach (var pool in _enemyPools.Values)
            pool.Clear();

        _enemyPools.Clear();
    }

    private void OnDestroy()
    {
        Reset();
    }
}
