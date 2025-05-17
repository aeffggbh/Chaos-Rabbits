using System;
using UnityEngine;

public class EnemyClassRandomizer : MonoBehaviour
{
    [SerializeField] public GameObject _jumpingEnemyPrefab;
    [SerializeField] public GameObject _normalEnemyPrefab;
    [SerializeField] public GameObject _explodingEnemyPrefab;

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

    private void AssignRandomClass()
    {
        int index = UnityEngine.Random.Range(0, enemyTypes.Length);
        Type chosenType = enemyTypes[index];

        //TODO: get rid of this.
        chosenType = typeof(ExplodingEnemy);

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
