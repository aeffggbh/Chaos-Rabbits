using UnityEngine;

/// <summary>
/// Randomizes the prefab of the enemy
/// </summary>
public class EnemyPrefabRandomizer : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;

    private void Start()
    {
        _enemyFactory.CreateEnemy(transform);
    }
}
