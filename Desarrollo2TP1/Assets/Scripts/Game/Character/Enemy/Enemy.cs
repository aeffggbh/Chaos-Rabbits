using System;
using System.Collections;
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
    protected Rigidbody _rb;
    protected Vector3 _targetWalk;
    protected Vector3 _targetLook;
    protected Vector3 _moveDir;
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
    protected PlayerMediator _playerMediator;
    protected States currentState = States.NONE;
    protected bool isExplodingEnemy;
    protected Coroutine _currentStateCoroutine;
    protected Coroutine _rangeCheckCoroutine;
    protected IChaseBehavior _chaseBehavior;
    protected IAttackBehavior _attackBehavior;
    protected IIdleBehavior _idleBehavior;
    protected IPatrolBehavior _patrolBehavior;
    protected IMovementBehavior _movementBehavior;

    protected override void Start()
    {
        base.Start();

        damage = 10.0f;

        IsWeaponUser = false;

        if (ServiceProvider.TryGetService<EnemyManager>(out var enemyManager))
            _manager = enemyManager;

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

        _idleTimer = _patrolTimer * 3;
        _idleCurrentTime = 0f;

        _timeSinceAttacked = 0;

        _moveSpeed = _patrolSpeed;

        isExplodingEnemy = gameObject.GetComponent<ExplodingEnemy>() != null;

        if (ServiceProvider.TryGetService<PlayerMediator>(out var playerController))
            _playerMediator = playerController;

        _chaseBehavior = this as IChaseBehavior;
        _attackBehavior = this as IAttackBehavior;
        _idleBehavior = this as IIdleBehavior;
        _patrolBehavior = this as IPatrolBehavior;
        _movementBehavior = this as IMovementBehavior;

        StartStateMachine();

    }

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

    private IEnumerator StartAttackCoroutine()
    {
        if (_attackBehavior != null)
        {
            _attackBehavior.ActivateAttack();

            _timeSinceAttacked = 0;

            while (currentState == States.ATTACK)
            {
                if (_playerMediator)
                {
                    _targetLook = _playerMediator.transform.position;
                    _attackBehavior.Attack();
                    _timeSinceAttacked += Time.deltaTime;
                }
                else
                {
                    if (ServiceProvider.TryGetService<PlayerMediator>(out var playerController))
                        _playerMediator = playerController;
                }
                yield return null;
            }
        }
    }

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

    private IEnumerator RangeCheckCoroutine()
    {
        while (true)
        {
            CheckRange();

            yield return new WaitForSeconds(0.1f);
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
        Vector3 playerDir = (_playerMediator.transform.position - transform.position).normalized;
        playerDir.y = 0;
        return playerDir;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_movementBehavior != null)
        {
            _counterMovement = new Vector3
                           (-_rb.linearVelocity.x * _manager.counterMovementForce,
                           0,
                           -_rb.linearVelocity.z * _manager.counterMovementForce);

            _movementBehavior.Move();

            LookAtTarget.Look(_targetLook, transform);
        }
    }

    /// <summary>
    /// Handles the chase state of the enemy, moving towards the player character.
    /// </summary>
    private void Chase()
    {
        //Debug.Log("Player direction:" + GetPlayerDirection());
        _moveDir = GetPlayerDirection();

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

    protected IEnumerator StartPatrolCoroutine()
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

        float start = Time.time;

        while (Vector3.Distance(transform.position, _targetWalk) > 0.5f &&
              (Time.time - start) < _patrolTimer)
            yield return null;

        SwitchState(States.IDLE);
    }

    private IEnumerator StartIdleCoroutine()
    {
        if (_idleBehavior != null)
        {
            _idleBehavior.ActivateIdle();

            _moveSpeed = 0;
            _moveDir = Vector3.zero;

            yield return new WaitForSeconds(_idleTimer);

            SwitchState(States.PATROL);
        }
    }

    public override void Die()
    {
        Debug.Log("DIE");

        _manager.enemies.Remove(this);

        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

}
