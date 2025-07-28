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

    protected IEnemyManager _manager;
    protected Vector3 _targetWalk;
    protected Vector3 _targetLook;
    protected float _attackRange;
    protected float _timeSinceAttacked;
    protected float _chaseRange;
    protected float _chasingSpeed;
    protected float _patrolSpeed;
    protected float _patrolTimer;
    protected float _patrolCurrentTime;
    protected float _idleTimer;
    protected float _idleCurrentTime;
    protected AnimationController animationController;
    protected PlayerMediator _playerMediator;

    protected States currentState = States.NONE;
    protected bool isExplodingEnemy;
    protected Coroutine _currentStateCoroutine;
    protected Coroutine _rangeCheckCoroutine;
    protected IChaseBehavior _chaseBehavior;
    protected IAttackBehavior _attackBehavior;
    protected IAttackActivationBehavior _attackActivationBehavior;
    protected IIdleBehavior _idleBehavior;
    protected IPatrolBehavior _patrolBehavior;
    protected IMovementBehavior _movementBehavior;

    protected float _currentSpeed;
    protected Vector3 _moveDir;
    protected Rigidbody _rb;
    protected Vector3 _counterMovement;
    protected float _acceleration;
    protected float _counterMovementForce;
    
    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public float CounterMovementForce { get => _counterMovementForce; set => _counterMovementForce = value; }
    public float RunSpeed { get => _chasingSpeed; set => _chasingSpeed = value; }
    public float WalkSpeed { get => _patrolSpeed; set => _patrolSpeed = value; }
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    protected override void Start()
    {
        base.Start();

        Damage = 10.0f;

        if (ServiceProvider.TryGetService<IEnemyManager>(out var enemyManager))
            _manager = enemyManager;

        Rb = gameObject.GetComponent<Rigidbody>();

        if (_manager != null)
        {
            _manager.Enemies.Add(this);
            _patrolTimer = _manager.PatrolTimer;
            _attackRange = _manager.AttackRange;
            _chaseRange = _manager.ChaseRange;
            _patrolSpeed = _manager.PatrolSpeed;
            _chasingSpeed = _manager.ChasingSpeed;
        }
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        _idleTimer = _patrolTimer * 3;
        _idleCurrentTime = 0f;

        _timeSinceAttacked = 0;

        _currentSpeed = _patrolSpeed;

        isExplodingEnemy = gameObject.GetComponent<ExplodingEnemy>() != null;

        _playerMediator = PlayerMediator.PlayerInstance;

        _chaseBehavior = this as IChaseBehavior;
        _attackBehavior = this as IAttackBehavior;
        _attackActivationBehavior = this as IAttackActivationBehavior;
        _idleBehavior = this as IIdleBehavior;
        _patrolBehavior = this as IPatrolBehavior;
        _movementBehavior = this as IMovementBehavior;

        StartStateMachine();

        EventTriggerManager.Trigger<IEnemySpawnEvent>(new EnemySpawnEvent(this, _manager, gameObject));
    }

    /// <summary>
    /// Starts the state machine
    /// </summary>
    private void StartStateMachine()
    {
        if (_rangeCheckCoroutine != null)
        {
            StopCoroutine(_rangeCheckCoroutine);
            _rangeCheckCoroutine = null;
        }

        _rangeCheckCoroutine = StartCoroutine(RangeCheckCoroutine());

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

        if (_currentStateCoroutine != null)
        {
            StopCoroutine(_currentStateCoroutine);
            _currentStateCoroutine = null;
        }

        currentState = state;

        switch (state)
        {
            case States.IDLE:
                _currentStateCoroutine = StartCoroutine(StartIdleCoroutine());
                break;
            case States.PATROL:
                _currentStateCoroutine = StartCoroutine(StartPatrolCoroutine());
                break;
            case States.CHASE:
                _currentStateCoroutine = StartCoroutine(StartChaseCoroutine());
                break;
            case States.ATTACK:
                _currentStateCoroutine = StartCoroutine(StartAttackCoroutine());
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
        if (_attackBehavior != null)
        {
            _attackActivationBehavior?.ActivateAttack();

            _timeSinceAttacked = 0;

            while (currentState == States.ATTACK)
            {
                if (_playerMediator)
                {
                    _targetLook = _playerMediator.transform.position;
                    _attackBehavior.Attack();
                    _timeSinceAttacked += Time.deltaTime;
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
        if (_chaseBehavior != null)
        {
            _chaseBehavior.ActivateChase();

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
        if (_playerMediator)
            return Vector3.Distance(transform.position, _playerMediator.transform.position);
        return 0;
    }

    /// <summary>
    /// Calculates the direction to the player character, ignoring the y-axis.
    /// </summary>
    /// <returns></returns>
    protected Vector3 GetPlayerDirection()
    {
        if (_playerMediator)
        {
            Vector3 playerDir = (_playerMediator.transform.position - transform.position).normalized;
            playerDir.y = 0;
            return playerDir;
        }

        return Vector3.zero;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_movementBehavior != null)
        {
            UpdateCounterMovement();

            _movementBehavior.Move();

            LookAtTarget.Look(_targetLook, transform);
        }
    }

    /// <summary>
    /// Updates the counter movement according to the force stored locally
    /// </summary>
    private void UpdateCounterMovement()
    {
        _counterMovement = new Vector3
                           (-Rb.linearVelocity.x * _manager.CounterMovementForce,
                           0,
                           -Rb.linearVelocity.z * _manager.CounterMovementForce);
    }

    /// <summary>
    /// Handles the chase state of the enemy, moving towards the player character.
    /// </summary>
    private void Chase()
    {
        _moveDir = GetPlayerDirection();

        if (_playerMediator != null)
            _targetLook = _playerMediator.transform.position;
    }

    /// <summary>
    /// Checks the distance to the player character and updates the enemy's state accordingly.
    /// </summary>
    private void CheckRange()
    {
        float distance = GetPlayerDistance();

        if (distance <= _attackRange && currentState != States.ATTACK)
            SwitchState(States.ATTACK);
        else if (distance > _attackRange && currentState != States.PATROL)
            currentState = States.NONE;

        if (currentState != States.CHASE && distance <= _chaseRange && currentState != States.ATTACK)
            SwitchState(States.CHASE);
        if (distance > _chaseRange && currentState != States.PATROL)
            SwitchState(States.PATROL);
    }

    /// <summary>
    /// Starts the patrolling logic
    /// </summary>
    /// <returns></returns>
    protected IEnumerator StartPatrolCoroutine()
    {
        currentState = States.PATROL;

        _patrolCurrentTime = 0;

        float randomZ = UnityEngine.Random.Range(-_manager.WalkRange, _manager.WalkRange);
        float randomX = UnityEngine.Random.Range(-_manager.WalkRange, _manager.WalkRange);

        _targetWalk = new Vector3(transform.position.x + randomX,
                                     transform.position.y,
                                     transform.position.z + randomZ);

        _targetLook = _targetWalk;
        _moveDir = (_targetWalk - transform.position).normalized;
        _moveDir.y = 0;
        _currentSpeed = _patrolSpeed;

        float start = Time.time;

        while (Vector3.Distance(transform.position, _targetWalk) > 0.5f &&
              (Time.time - start) < _patrolTimer)
            yield return null;

        SwitchState(States.IDLE);
    }

    /// <summary>
    /// Starts the idle logic
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartIdleCoroutine()
    {
        if (_idleBehavior != null)
        {
            _idleBehavior.ActivateIdle();

            _currentSpeed = 0;
            _moveDir = Vector3.zero;

            yield return new WaitForSeconds(_idleTimer);

            SwitchState(States.PATROL);
        }
    }

    /// <summary>
    /// Removes this enemy from the enemy list before dying
    /// </summary>
    public override void Die()
    {
        _manager.Enemies.Remove(this);
        
        EventTriggerManager.Trigger<IEnemyDespawnEvent>(new EnemyDespawnEvent(this, _manager, gameObject));

        base.Die();
    }
}
