using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public List<Enemy> enemies;

    private void Start()
    {
        if (enemies.Count == 0)
            Debug.LogWarning("No enemies added!");
    }
}
