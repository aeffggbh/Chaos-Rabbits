using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for all enemies in the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : Character, IPhysicsMovementData
{
    protected enum States
    {
        NONE,
        IDLE,
        PATROL,
        CHASE,
        ATTACK
    }

    protected IEnemyManager manager;
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

    protected States currentState = States.NONE;
    protected bool isExplodingEnemy;
    protected Coroutine currentStateCoroutine;
    protected Coroutine rangeCheckCoroutine;
    protected IChaseBehavior chaseBehavior;
    protected IAttackBehavior attackBehavior;
    protected IAttackActivationBehavior attackActivationBehavior;
    protected IIdleBehavior idleBehavior;
    protected IPatrolBehavior patrolBehavior;
    protected IMovementBehavior movementBehavior;

    protected float currentSpeed;
    protected Vector3 moveDir;
    protected Rigidbody rb;
    protected Vector3 counterMovement;
    protected float acceleration;
    protected float counterMovementForce;

    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float CounterMovementForce { get => counterMovementForce; set => counterMovementForce = value; }
    public float RunSpeed { get => chasingSpeed; set => chasingSpeed = value; }
    public float WalkSpeed { get => patrolSpeed; set => patrolSpeed = value; }
    public Rigidbody Rb { get => rb; set => rb = value; }

    protected override void Start()
    {
        base.Start();

        Damage = 10.0f;

        if (ServiceProvider.TryGetService<IEnemyManager>(out var enemyManager))
            manager = enemyManager;

        Rb = gameObject.GetComponent<Rigidbody>();

        if (manager != null)
        {
            manager.Enemies.Add(this);
            patrolTimer = manager.PatrolTimer;
            attackRange = manager.AttackRange;
            chaseRange = manager.ChaseRange;
            patrolSpeed = manager.PatrolSpeed;
            chasingSpeed = manager.ChasingSpeed;
        }
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        idleTimer = patrolTimer * 3;
        idleCurrentTime = 0f;

        timeSinceAttacked = 0;

        currentSpeed = patrolSpeed;

        isExplodingEnemy = gameObject.GetComponent<ExplodingEnemy>() != null;

        ServiceProvider.TryGetService<PlayerMediator>(out playerMediator);

        chaseBehavior = this as IChaseBehavior;
        attackBehavior = this as IAttackBehavior;
        attackActivationBehavior = this as IAttackActivationBehavior;
        idleBehavior = this as IIdleBehavior;
        patrolBehavior = this as IPatrolBehavior;
        movementBehavior = this as IMovementBehavior;

        StartStateMachine();

        EventTriggerer.Trigger<IEnemySpawnEvent>(new EnemySpawnEvent(this, manager, gameObject));
    }

    /// <summary>
    /// Starts the state machine
    /// </summary>
    private void StartStateMachine()
    {
        if (rangeCheckCoroutine != null)
        {
            StopCoroutine(rangeCheckCoroutine);
            rangeCheckCoroutine = null;
        }

        rangeCheckCoroutine = StartCoroutine(RangeCheckCoroutine());

        SwitchState(States.PATROL);
    }

    /// <summary>
    /// Switches to another state
    /// </summary>
    /// <param name="state"></param>
    private void SwitchState(States state)
    {
        if (state == currentState)
            return;

        if (currentStateCoroutine != null)
        {
            StopCoroutine(currentStateCoroutine);
            currentStateCoroutine = null;
        }

        currentState = state;

        switch (state)
        {
            case States.IDLE:
                currentStateCoroutine = StartCoroutine(StartIdleCoroutine());
                break;
            case States.PATROL:
                currentStateCoroutine = StartCoroutine(StartPatrolCoroutine());
                break;
            case States.CHASE:
                currentStateCoroutine = StartCoroutine(StartChaseCoroutine());
                break;
            case States.ATTACK:
                currentStateCoroutine = StartCoroutine(StartAttackCoroutine());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Executes attack logic
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartAttackCoroutine()
    {
        if (attackBehavior != null)
        {
            attackActivationBehavior?.ActivateAttack();

            timeSinceAttacked = 0;

            while (currentState == States.ATTACK)
            {
                if (playerMediator)
                {
                    targetLook = playerMediator.transform.position;
                    attackBehavior.Attack();
                    timeSinceAttacked += Time.deltaTime;
                }

                yield return null;
            }
        }
    }

    /// <summary>
    /// Executes chasing logic
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartChaseCoroutine()
    {
        if (chaseBehavior != null)
        {
            chaseBehavior.ActivateChase();

            while (currentState == States.CHASE)
            {
                Chase();
                yield return null;
            }
        }

    }

    /// <summary>
    /// Checks the range from the enemy to the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator RangeCheckCoroutine()
    {
        while (true)
        {
            CheckRange();

            yield return null;
        }
    }

    /// <summary>
    /// Calculates the distance to the player character.
    /// </summary>
    /// <returns></returns>
    protected float GetPlayerDistance()
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

        if (movementBehavior != null)
        {
            UpdateCounterMovement();

            movementBehavior.Move();

            LookAtTarget.Look(targetLook, transform);
        }
    }

    /// <summary>
    /// Updates the counter movement according to the force stored locally
    /// </summary>
    private void UpdateCounterMovement()
    {
        float counterForce = moveDir.magnitude > 0.1f ? manager.CounterMovementForce :
            manager.CounterMovementForce * 2;

        counterMovement = new Vector3
                           (-Rb.linearVelocity.x * counterForce,
                           0,
                           -Rb.linearVelocity.z * counterForce);
    }

    /// <summary>
    /// Handles the chase state of the enemy, moving towards the player character.
    /// </summary>
    private void Chase()
    {
        moveDir = GetPlayerDirection();

        if (playerMediator != null)
            targetLook = playerMediator.transform.position;
    }

    /// <summary>
    /// Checks the distance to the player character and updates the enemy's state accordingly.
    /// </summary>
    private void CheckRange()
    {
        float distance = GetPlayerDistance();

        if (distance <= attackRange && currentState != States.ATTACK)
            SwitchState(States.ATTACK);
        else if (distance > attackRange && currentState != States.PATROL)
            currentState = States.NONE;

        if (currentState != States.CHASE && distance <= chaseRange && currentState != States.ATTACK)
            SwitchState(States.CHASE);
        if (distance > chaseRange && currentState != States.PATROL)
            SwitchState(States.PATROL);
    }

    /// <summary>
    /// Starts the patrolling logic
    /// </summary>
    /// <returns></returns>
    protected IEnumerator StartPatrolCoroutine()
    {
        currentState = States.PATROL;

        patrolCurrentTime = 0;

        float randomZ = UnityEngine.Random.Range(-manager.WalkRange, manager.WalkRange);
        float randomX = UnityEngine.Random.Range(-manager.WalkRange, manager.WalkRange);

        Vector3 dir = new(randomX, 0, randomZ);
        dir = dir.normalized;

        float distance = manager.WalkRange;

        if (RayManager.PointingToObject(transform, distance, out RaycastHit hitInfo, dir))
            distance = hitInfo.distance * 0.8f;

        targetWalk = transform.position + (dir * distance);

        targetLook = targetWalk;
        moveDir = dir;
        moveDir.y = 0;
        currentSpeed = patrolSpeed;

        float start = Time.time;
        float distanceThreshold = 0.3f;

        while (currentState == States.PATROL &&
              (Time.time - start) < patrolTimer)
        {
            float distanceToTarget = Vector3.Distance(
                new(transform.position.x, 0, transform.position.z),
                new(targetWalk.x, 0, targetWalk.z));

            if (distanceToTarget < distanceThreshold)
            {
                SwitchState(States.IDLE);
                yield break;
            }

            yield return null;
        }

        SwitchState(States.IDLE);
    }

    /// <summary>
    /// Starts the idle logic
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartIdleCoroutine()
    {
        if (idleBehavior != null)
        {
            idleBehavior.ActivateIdle();

            currentSpeed = 0;
            moveDir = Vector3.zero;

            yield return new WaitForSeconds(idleTimer);

            SwitchState(States.PATROL);
        }
    }

    /// <summary>
    /// Removes this enemy from the enemy list before dying
    /// </summary>
    public override void Die()
    {
        manager.Enemies.Remove(this);

        EventTriggerer.Trigger<IEnemyDespawnEvent>(new EnemyDespawnEvent(this, manager, gameObject));

        base.Die();
    }
}
