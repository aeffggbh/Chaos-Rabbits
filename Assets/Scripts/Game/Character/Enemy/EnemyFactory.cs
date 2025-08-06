using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Factory for random enemies
/// </summary>
[CreateAssetMenu(fileName = "EnemyFactory", menuName = "ScriptableObjects/Factory/EnemyFactory")]
public class EnemyFactory : ScriptableObject
{
    [SerializeField] private List<EnemyData> _enemyData;

    /// <summary>
    /// Instantiates a random enemy
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public Enemy CreateEnemy(Transform parent)
    {
        int index = GetRandomIndex(parent);
        var prefab = _enemyData[index].Prefab;
        var stats = _enemyData[index].Stats;

        if (prefab == null)
        {
            Debug.LogError("Prefab at " + index + " is null");
            return null;
        }
        else if (stats == null)
        {
            Debug.LogError("Stats at " + index + " is null");
            return null;
        }

        var instance = Instantiate(prefab, parent);

        Enemy enemy = instance.GetComponent<Enemy>();
        enemy.Stats = stats;

        return enemy;
    }

    /// <summary>
    /// Gets a random index from the enemy data list
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    private int GetRandomIndex(Transform parent)
    {
        return UnityEngine.Random.Range(0, _enemyData.Count);
    }
}
