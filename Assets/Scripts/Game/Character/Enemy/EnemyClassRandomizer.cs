using System;
using UnityEngine;

/// <summary>
/// Randomly assigns an enemy class to the GameObject this script is attached to.
/// </summary>
public class EnemyClassRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject _jumpingEnemyPrefabGO;
    [SerializeField] private GameObject _normalEnemyPrefabGO;
    [SerializeField] private GameObject _explodingEnemyPrefabGO;
    [SerializeField] private Type debugType = typeof(JumpingEnemy);

    /// <summary>
    /// Array of enemy types to randomly choose from.
    /// </summary>
    private Type[] enemyTypes = new Type[]
    {
        typeof(JumpingEnemy),
        typeof(ShooterEnemy),
        typeof(ExplodingEnemy)
    };

    private void Awake()
    {
        AssignRandomClass();
    }

    /// <summary>
    /// Assigns a random enemy class to this GameObject by instantiating the corresponding prefab
    /// </summary>
    private void AssignRandomClass()
    {
        int index = UnityEngine.Random.Range(0, enemyTypes.Length);
        Type chosenType = enemyTypes[index];

        if (debugType != null)
            chosenType = debugType;

        GameObject modelGO = null;

        if (chosenType == typeof(JumpingEnemy))
            modelGO = Instantiate(_jumpingEnemyPrefabGO, transform);
        else if (chosenType == typeof(ShooterEnemy))
            modelGO = Instantiate(_normalEnemyPrefabGO, transform);
        else if (chosenType == typeof(ExplodingEnemy))
            modelGO = Instantiate(_explodingEnemyPrefabGO, transform);

        if (modelGO == null)
            Debug.LogError("No model found in " + gameObject.name);
        else
            modelGO.AddComponent(chosenType);
    }
}
