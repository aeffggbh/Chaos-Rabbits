using System.Collections.Generic;
using UnityEngine;

//TODO: adjust this

/// <summary>
/// Manages all enemies in the game.
/// </summary>
public class EnemyManager : MonoBehaviour, IEnemyManager
{
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private float attackTimer;
    [SerializeField] private float walkRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float patrolTimer;
    [SerializeField] private float counterMovementForce;
    public List<Enemy> Enemies => enemies;
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float WalkRange { get => walkRange; set => walkRange = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ChaseRange { get => chaseRange; set => chaseRange = value; }
    public float ChasingSpeed { get => chasingSpeed; set => chasingSpeed = value; }
    public float PatrolSpeed { get => patrolSpeed; set => patrolSpeed = value; }
    public float PatrolTimer { get => patrolTimer; set => patrolTimer = value; }
    public float CounterMovementForce { get => counterMovementForce; set => counterMovementForce = value; }

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
