using System.Collections.Generic;
using UnityEngine;

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

    public static EnemyManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        if (enemies.Count == 0)
            Debug.LogWarning("No enemies added!");

    }
}
