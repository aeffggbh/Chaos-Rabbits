using System;
using UnityEngine;

/// <summary>
/// Base class for all enemies in the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : Character
{
    protected EnemyStats stats;
    protected Vector3 targetWalk;
    protected Vector3 targetLook;
    protected float attackRange;
    protected float timeSinceAttacked;
    protected float chaseRange;
    protected float chasingSpeed;
    protected float patrolSpeed;
    protected float patrolTimer;
    protected float patrolCurrentTime;
    protected float idleTimer;
    protected float idleCurrentTime;
    protected AnimationController animationController;
    protected PlayerMediator playerMediator;

    protected Coroutine currentStateCoroutine;
    protected Coroutine rangeCheckCoroutine;

    protected float currentSpeed;
    protected Vector3 moveDir;
    protected Rigidbody rb;
    protected Vector3 counterMovement;
    protected float acceleration;
    protected float counterMovementForce;

    private IEnemyState currentState;
    private Action _release;
    private GameObject _prefabSource;

    public EnemyStats Stats { get => stats; set => stats = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public PlayerMediator PlayerMediator => playerMediator;
    public Vector3 TargetLook { get => targetLook; set => targetLook = value; }
    public float TimeSinceAttacked { get => timeSinceAttacked; set => timeSinceAttacked = value; }
    public GameObject PrefabSource { get => _prefabSource; set => _prefabSource = value; }

    protected override void Start()
    {
        base.Start();

        if (!stats)
            Debug.LogError("No stats on " + gameObject.name);

        Damage = 10.0f;

        if (ServiceProvider.TryGetService<IEnemyContainer>(out var enemyManager))

            rb = gameObject.GetComponent<Rigidbody>();

        if (enemyManager != null)
        {
            enemyManager.Add(this);
            patrolTimer = stats.PatrolTimer;
            attackRange = stats.AttackRange;
            chaseRange = stats.ChaseRange;
            patrolSpeed = stats.PatrolSpeed;
            chasingSpeed = stats.ChasingSpeed;
        }
        else
            Debug.LogError(nameof(EnemyContainer) + " is null");

        idleTimer = patrolTimer * 3;
        idleCurrentTime = 0f;

        timeSinceAttacked = 0;

        currentSpeed = patrolSpeed;

        ServiceProvider.TryGetService<PlayerMediator>(out playerMediator);

        ChangeState(new PatrolState(this));

        EventTriggerer.Trigger<IEnemySpawnEvent>(new EnemySpawnEvent(this, enemyManager, gameObject));
    }

    /// <summary>
    /// Sets the function that happens when the enemy is released.
    /// </summary>
    /// <param name="action"></param>
    public void SetPoolReleaseAction(Action action)
    {
        _release = action;
    }

    public virtual void Reset()
    {
        timeSinceAttacked = 0;
        currentSpeed = patrolSpeed;
        moveDir = Vector3.zero;
        targetLook = Vector3.zero;
        StopAllCoroutines();

        ChangeState(new PatrolState(this));
    }

    /// <summary>
    /// Calculates the distance to the player character.
    /// </summary>
    /// <returns></returns>
    public float GetPlayerDistance()
    {
        if (playerMediator)
            return Vector3.Distance(transform.position, playerMediator.transform.position);
        return 0;
    }

    /// <summary>
    /// Calculates the direction to the player character, ignoring the y-axis.
    /// </summary>
    /// <returns></returns>
    protected Vector3 GetPlayerDirection()
    {
        if (playerMediator)
        {
            Vector3 playerDir = (playerMediator.transform.position - transform.position).normalized;
            playerDir.y = 0;
            return playerDir;
        }

        return Vector3.zero;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        currentState?.CheckRange();

        if (this is IMovementBehavior)
        {
            UpdateCounterMovement();

            (this as IMovementBehavior).Move();

            LookAtTarget.Look(targetLook, transform);
        }
    }

    /// <summary>
    /// Updates the counter movement according to the force stored locally
    /// </summary>
    private void UpdateCounterMovement()
    {
        float counterForce = moveDir.magnitude > 0.1f ? stats.CounterMovementForce :
            stats.CounterMovementForce * 2;

        counterMovement = new Vector3
                           (-rb.linearVelocity.x * counterForce,
                           0,
                           -rb.linearVelocity.z * counterForce);
    }

    /// <summary>
    /// Handles the chase state of the enemy, moving towards the player character.
    /// </summary>
    public void Chase()
    {
        moveDir = GetPlayerDirection();

        if (playerMediator != null)
            targetLook = playerMediator.transform.position;
    }


    /// <summary>
    /// Removes this enemy from the enemy list before dying
    /// </summary>
    public override void Die()
    {
        if (!ServiceProvider.TryGetService<IEnemyContainer>(out var enemyManager))
            return;

        enemyManager.Enemies.Remove(this);

        EventTriggerer.Trigger<IEnemyDespawnEvent>(new EnemyDespawnEvent(this, enemyManager, gameObject));

        if (_release != null)
            _release?.Invoke();
    }

    internal void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
