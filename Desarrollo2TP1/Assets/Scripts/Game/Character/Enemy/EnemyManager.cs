using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all enemies in the game.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] public List<Enemy> enemies;
    [SerializeField] public float attackTimer;
    [SerializeField] public float walkRange;
    [SerializeField] public float attackRange;
    [SerializeField] public float chaseRange;
    [SerializeField] public float chasingSpeed;
    [SerializeField] public float patrolSpeed;
    [SerializeField] public float patrolTimer;
    [SerializeField] public float counterMovementForce;

    private void Awake()
    {
        ServiceProvider.SetService<EnemyManager>(this, true);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<EnemyManager>(null, true);
    }

    private void Start()
    {
        if (enemies.Count == 0)
            Debug.LogWarning("No enemies added!");

    }
}
