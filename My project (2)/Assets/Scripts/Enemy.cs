using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    public EnemyManager _manager;
    protected PlayerController _playerController;
    protected Transform _head;
    protected Rigidbody _rb;
    protected Vector3 _targetWalk;
    protected Vector3 _targetLook;
    protected Vector3 _moveDir;
    protected bool _hasAttacked;
    protected bool _isPatrolling;
    protected bool _isChasing;
    protected bool _isAttacking;
    protected bool _pausedPatrol;
    protected float _moveSpeed;
    protected float _attackRange;
    protected float _timeSinceAttacked;
    protected float _chaseRange;
    protected float _chasingSpeed;
    /// <summary>
    /// every few seconds it'll change the direction of patrolling
    /// </summary>
    protected float _patrolTimer;
    protected float _patrolSpeed;
    protected float _patrolCurrentTime;
    protected Vector3 _counterMovement;


    virtual protected void Start()
    {
        _manager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        _head = transform.Find("head"); ;

        _rb = gameObject.GetComponent<Rigidbody>();

        if (_manager != null)
        {
            _manager._enemies.Add(this);
            _patrolTimer = _manager._patrolTimer;
            _attackRange = _manager._attackRange;
            _chaseRange = _manager._chaseRange;
            _playerController = _manager._playerController;
            _patrolSpeed = _manager._patrolSpeed;
            _chasingSpeed = _manager._chasingSpeed;
        }
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        _isPatrolling = false;
        _isChasing = false;
        _isAttacking = false;
        _hasAttacked = false;
        _pausedPatrol = false;


        ActivatePatrol();
    }

    protected float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, _playerController.transform.position);
    }

    protected Vector3 GetPlayerDirection()
    {
        Vector3 playerDir = (_playerController.transform.position - transform.position).normalized;
        //para que no vuele
        playerDir.y = 0;
        return playerDir;
    }

    virtual protected void FixedUpdate()
    {
        _counterMovement = new Vector3
                       (-_rb.linearVelocity.x * _manager._counterMovementForce,
                       0,
                       -_rb.linearVelocity.z * _manager._counterMovementForce);

        Move();

        //Debug.Log("dir: " + _moveDir);
        //Debug.Log("speed: " + _moveSpeed);

        _patrolCurrentTime += Time.fixedDeltaTime;
        if (_hasAttacked)
            _timeSinceAttacked += Time.fixedDeltaTime;

        if (!_isChasing && GetPlayerDistance() <= _chaseRange)
        {
            ActivateChase();
            _isChasing = true;
        }
        if (GetPlayerDistance() >= _chaseRange && !_isPatrolling)
        {
            _isPatrolling = true;
            ActivatePatrol();
            if (_isChasing)
                _isChasing = false;
        }

        if (_isChasing || _isAttacking)
        {
            _targetLook = _playerController.transform.position * 5;
            //_targetLook.y = transform.position.y;
        }

        if (_isChasing)
        {
            //Debug.Log("Player direction:" + GetPlayerDirection());
            _moveDir = GetPlayerDirection();

            if (GetPlayerDistance() <= _attackRange)
            {
                _isChasing = false;
                _isAttacking = true;
            }
        }
        else if (_isPatrolling)
        {

            if (_pausedPatrol && _patrolCurrentTime >= _patrolTimer)
            {
                _pausedPatrol = false;
                ActivatePatrol();
            }
            else if (_patrolCurrentTime >= _patrolTimer)
                _pausedPatrol = true;

            if (Vector3.Distance(transform.position, _targetWalk) < 0.1 && !_pausedPatrol)
            {
                _pausedPatrol = true;
                _patrolCurrentTime = 0;
                _moveSpeed = 0;
                _moveDir = Vector3.zero;
            }
            //Debug.Log("PATROLLING");
        }

        if (_isAttacking)
        {
            //Debug.Log("ATTACKING");
            Attack();
        }

        transform.LookAt(_targetLook);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _moveDir * 2);
    }

    void ActivatePatrol()
    {
        _patrolCurrentTime = 0;
        float randomZ = UnityEngine.Random.Range(-_manager._walkRange, _manager._walkRange);
        float randomX = UnityEngine.Random.Range(-_manager._walkRange, _manager._walkRange);

        _targetWalk = new Vector3(transform.position.x + randomX,
                                     transform.position.y,
                                     transform.position.z + randomZ);

        _targetLook = _targetWalk * 2;


        _moveDir = (_targetWalk - transform.position).normalized;
        _moveDir.y = 0;
        _moveSpeed = _patrolSpeed;

        if (!_isPatrolling)
            _isPatrolling = true;
    }

    protected abstract void Move();

    protected abstract void ActivateChase();

    protected abstract void Attack();

    public void Die()
    {
        //maybe take damage instead, lol
        Debug.Log("DIE");
        gameObject.SetActive(false);
    }

}
