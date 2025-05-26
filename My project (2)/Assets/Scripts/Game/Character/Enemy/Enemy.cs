using UnityEngine;

/// <summary>
/// Base class for all enemies in the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : Character
{
    protected enum States
    {
        NONE,
        IDLE,
        PATROL,
        CHASE,
        ATTACK
    }

    public EnemyManager _manager;
    public BoxCollider _collider;
    protected Transform _head;
    protected Rigidbody _rb;
    protected Vector3 _targetWalk;
    protected Vector3 _targetLook;
    protected Vector3 _moveDir;
    protected bool _pausedPatrol;
    protected float _moveSpeed;
    protected float _attackRange;
    protected float _timeSinceAttacked;
    protected float _chaseRange;
    protected float _chasingSpeed;
    protected float _patrolSpeed;
    protected float _patrolTimer;
    protected float _patrolCurrentTime;
    protected float _idleTimer;
    protected float _idleCurrentTime;
    protected Vector3 _counterMovement;
    protected AnimationController animationController;
    protected PlayerController _playerController;
    protected LookAtTarget _lookAtTrarget;
    protected States currentState = States.PATROL;
    protected bool isExplodingEnemy;

    protected override void Start()
    {
        base.Start();

        maxHealth = 100.0f;
        currentHealth = maxHealth;

        damage = 10.0f;

        IsWeaponUser = false;

        if (ServiceProvider.TryGetService<EnemyManager>(out var enemyManager))
            _manager = enemyManager;

        _collider = gameObject.GetComponent<BoxCollider>();
        _head = transform.Find("head"); ;
        _lookAtTrarget = gameObject.AddComponent<LookAtTarget>();

        _rb = gameObject.GetComponent<Rigidbody>();

        if (_manager != null)
        {
            _manager.enemies.Add(this);
            _patrolTimer = _manager.patrolTimer;
            _attackRange = _manager.attackRange;
            _chaseRange = _manager.chaseRange;
            _patrolSpeed = _manager.patrolSpeed;
            _chasingSpeed = _manager.chasingSpeed;
        }
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        _pausedPatrol = false;

        _idleTimer = _patrolTimer;
        _idleCurrentTime = 0f;

        _timeSinceAttacked = 0;

        _moveSpeed = _patrolSpeed;

        ActivatePatrol();

        isExplodingEnemy = this.GetComponent<ExplodingEnemy>() != null;

        if (ServiceProvider.TryGetService<PlayerController>(out var playerController))
            _playerController = playerController;
    }

    /// <summary>
    /// Calculates the distance to the player character.
    /// </summary>
    /// <returns></returns>
    protected float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, _playerController.transform.position);
    }

    /// <summary>
    /// Calculates the direction to the player character, ignoring the y-axis.
    /// </summary>
    /// <returns></returns>
    protected Vector3 GetPlayerDirection()
    {
        Vector3 playerDir = (_playerController.transform.position - transform.position).normalized;
        playerDir.y = 0;
        return playerDir;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _counterMovement = new Vector3
                       (-_rb.linearVelocity.x * _manager.counterMovementForce,
                       0,
                       -_rb.linearVelocity.z * _manager.counterMovementForce);
        Move();

        CheckTimers();

        CheckRange();

        switch (currentState)
        {
            case States.IDLE:
                Idle();
                break;
            case States.PATROL:
                Patrol();
                break;
            case States.CHASE:
                Chase();
                break;
            case States.ATTACK:
                Attack();
                _targetLook = _playerController.transform.position;
                break;
            default:
                break;
        }

        if (!isExplodingEnemy)
            _lookAtTrarget.Look(_targetLook);
    }

    /// <summary>
    /// Handles the idle state of the enemy.
    /// </summary>
    private void Idle()
    {
        if (_idleCurrentTime > _idleTimer)
        {
            _idleCurrentTime = 0;
            currentState = States.PATROL;
            ActivatePatrol();
        }
    }

    /// <summary>
    /// Handles the patrol state of the enemy.
    /// </summary>
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _targetWalk) < 0.5f || _patrolCurrentTime > _patrolTimer)
        {
            _patrolCurrentTime = 0;
            _moveSpeed = 0;
            _moveDir = Vector3.zero;
            currentState = States.IDLE;
            ActivateIdle();
        }
    }

    /// <summary>
    /// Checks and updates the timers for different states of the enemy.
    /// </summary>
    private void CheckTimers()
    {
        switch (currentState)
        {
            case States.IDLE:
                _idleCurrentTime += Time.fixedDeltaTime;
                break;
            case States.PATROL:
                _patrolCurrentTime += Time.fixedDeltaTime;
                break;
            case States.CHASE:
                break;
            case States.ATTACK:
                _timeSinceAttacked += Time.fixedDeltaTime;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Handles the chase state of the enemy, moving towards the player character.
    /// </summary>
    private void Chase()
    {
        //Debug.Log("Player direction:" + GetPlayerDirection());
        _moveDir = GetPlayerDirection();

        _targetLook = _playerController.transform.position;
    }

    /// <summary>
    /// Checks the distance to the player character and updates the enemy's state accordingly.
    /// </summary>
    private void CheckRange()
    {
        if (GetPlayerDistance() <= _attackRange && currentState != States.ATTACK)
        {
            _timeSinceAttacked = 0;
            ActivateAttack();
            currentState = States.ATTACK;
        }
        else if (GetPlayerDistance() > _attackRange && currentState != States.PATROL)
            currentState = States.NONE;

        if (currentState != States.CHASE && GetPlayerDistance() <= _chaseRange && currentState != States.ATTACK)
        {
            currentState = States.CHASE;
            ActivateChase();
        }
        if (GetPlayerDistance() > _chaseRange && currentState != States.PATROL)
        {
            if (currentState != States.IDLE)
            {
                currentState = States.PATROL;
                ActivatePatrol();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _moveDir * 2);
    }

    /// <summary>
    /// Activates the idle state of the enemy, where it waits before patrolling again.
    /// </summary>
    protected abstract void ActivateIdle();

    /// <summary>
    /// Activates the patrol state of the enemy, where it moves to a random point within a defined range.
    /// </summary>
    virtual protected void ActivatePatrol()
    {
        currentState = States.PATROL;
        _patrolCurrentTime = 0;
        float randomZ = UnityEngine.Random.Range(-_manager.walkRange, _manager.walkRange);
        float randomX = UnityEngine.Random.Range(-_manager.walkRange, _manager.walkRange);

        _targetWalk = new Vector3(transform.position.x + randomX,
                                     transform.position.y,
                                     transform.position.z + randomZ);

        _targetLook = _targetWalk;

        _moveDir = (_targetWalk - transform.position).normalized;
        _moveDir.y = 0;
        _moveSpeed = _patrolSpeed;
    }

    /// <summary>
    /// Manages the movement behaviour of the enemy, which can vary based on the type of enemy.
    /// </summary>
    protected abstract void Move();

    /// <summary>
    /// Activates the chase state of the enemy, where it usually moves towards the player character.
    /// </summary>
    protected abstract void ActivateChase();

    /// <summary>
    /// Handles the attack logic of the enemy, which can vary based on the type of enemy.
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    /// Activates the attack state of the enemy, which is called when the enemy is close enough to the player character.
    /// </summary>
    protected abstract void ActivateAttack();

    public override void Die()
    {
        Debug.Log("DIE");

        _manager.enemies.Remove(this);
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

}
