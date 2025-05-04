using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    public EnemyManager _manager;
    protected PlayerController _playerController;
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
    protected float _chaseRange;
    protected float _timeSinceAttacked;
    /// <summary>
    /// every few seconds it'll change the direction of patrolling
    /// </summary>
    protected float _patrolTimer;
    protected float _patrolSpeed;
    protected float _patrolCurrentTime;
    protected Vector3 _counterMovement;


    private void Start()
    {
        _manager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        _rb = gameObject.GetComponent<Rigidbody>();

        if (_manager != null)
        {
            _manager._enemies.Add(this);
            _patrolTimer = _manager._patrolTimer;
            _attackRange = _manager._attackRange;
            _chaseRange = _manager._chaseRange;
            _playerController = _manager._playerController;
            _patrolSpeed = _manager._patrolSpeed;
        }
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        _isPatrolling = true;
        _isChasing = false;
        _isAttacking = false;
        _hasAttacked = false;
        _pausedPatrol = false;


        ActivatePatrol();
    }

    protected float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, _playerController.transform.position);

        //Vector3 start = transform.position;
        //Vector3 end = _playerController.transform.position;
        //float diffX = end.x - start.x;
        //float diffY = end.y - start.y;
        //float diffZ = end.z - start.z;
        //return (float)Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ);
    }

    protected Vector3 GetPlayerDirection()
    {
        Vector3 playerDir = (_targetWalk - transform.position).normalized;
        //para que no vuele
        playerDir.y = 0;
        return playerDir;
    }

    private void FixedUpdate()
    {
        _counterMovement = new Vector3
                       (-_rb.linearVelocity.x * _manager._counterMovementForce,
                       0,
                       -_rb.linearVelocity.z * _manager._counterMovementForce);

        //check if the player is on sight and if it's in the attack range.
        _rb.AddForce((_moveDir * _moveSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);


        Debug.Log("dir: " + _moveDir);
        Debug.Log("speed: " + _moveSpeed);

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
            _targetLook = _playerController.GetCinemachineCamera().position;

        if (_isChasing)
        {
            Debug.Log("CHASING");
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
            {
                _pausedPatrol = true;
            }

            if (Vector3.Distance(transform.position, _targetWalk) < 0.1 && !_pausedPatrol)
            {
                _pausedPatrol = true;
                _patrolCurrentTime = 0;
                _moveSpeed = 0;
                _moveDir = Vector3.zero;
            }
            Debug.Log("PATROLLING");
        }
        else if (_isAttacking)
        {
            Debug.Log("ATTACKING");
            Attack();
        }

        transform.LookAt(_targetLook);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _moveDir * 2);
    }

    protected abstract void ActivatePatrol();

    protected abstract void ActivateChase();

    protected abstract void Attack();

    public void Die()
    {
        //maybe take damage instead, lol
        Debug.Log("DIE");
        gameObject.SetActive(false);
    }

}
