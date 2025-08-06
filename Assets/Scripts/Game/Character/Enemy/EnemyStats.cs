using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float attackTimer;
    [SerializeField] private float walkRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float patrolTimer;
    [SerializeField] private float counterMovementForce;
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public float WalkRange { get => walkRange; set => walkRange = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ChaseRange { get => chaseRange; set => chaseRange = value; }
    public float ChasingSpeed { get => chasingSpeed; set => chasingSpeed = value; }
    public float PatrolSpeed { get => patrolSpeed; set => patrolSpeed = value; }
    public float PatrolTimer { get => patrolTimer; set => patrolTimer = value; }
    public float CounterMovementForce { get => counterMovementForce; set => counterMovementForce = value; }
}