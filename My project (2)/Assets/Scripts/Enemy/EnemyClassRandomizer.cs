using System;
using UnityEngine;

public class EnemyClassRandomizer : MonoBehaviour
{
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
        int index = UnityEngine.Random.Range(0, enemyTypes.Length - 1);
        Type chosenType = enemyTypes[index];

        //TODO: saca esto
        chosenType = typeof(JumpingEnemy);

        gameObject.AddComponent(chosenType);

    }
}
