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

        if (debugType != null)
            chosenType = debugType;

        GameObject modelInstanceGO = null;

        if (chosenType == typeof(JumpingEnemy))
            modelInstanceGO = Instantiate(_jumpingEnemyPrefabGO, transform);
        else if (chosenType == typeof(NormalEnemy))
            modelInstanceGO = Instantiate(_normalEnemyPrefabGO, transform);
        else if (chosenType == typeof(ExplodingEnemy))
            modelInstanceGO = Instantiate(_explodingEnemyPrefabGO, transform);

        if (modelInstanceGO == null)
            Debug.LogError("No model found in " + gameObject.name);
        else
            modelInstanceGO.AddComponent(chosenType);
    }
}
