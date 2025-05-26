using System;
using UnityEngine;

/// <summary>
/// Randomly assigns an enemy class to the GameObject this script is attached to.
/// </summary>
public class EnemyClassRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject _jumpingEnemyPrefab;
    [SerializeField] private GameObject _normalEnemyPrefab;
    [SerializeField] private GameObject _explodingEnemyPrefab;

    /// <summary>
    /// Array of enemy types to randomly choose from.
    /// </summary>
    private Type[] enemyTypes = new Type[]
    {
        typeof(JumpingEnemy),
        typeof(NormalEnemy),
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

        // Instantiate and parent the correct model
        GameObject modelInstance = null;

        if (chosenType == typeof(JumpingEnemy))
            modelInstance = Instantiate(_jumpingEnemyPrefab, transform);
        else if (chosenType == typeof(NormalEnemy))
            modelInstance = Instantiate(_normalEnemyPrefab, transform);
        else if (chosenType == typeof(ExplodingEnemy))
            modelInstance = Instantiate(_explodingEnemyPrefab, transform);

        if (modelInstance == null)
            Debug.LogError("No model found in " + gameObject.name);
        else
            modelInstance.AddComponent(chosenType);
    }
}
