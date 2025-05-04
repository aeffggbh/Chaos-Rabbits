using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    public EnemyManager _manager;
    public GameObject _originalObject;
    //reference for the player.
    protected PlayerController _playerController;
    protected float _timeSinceAttacked;
    protected bool _hasAttacked;
    protected Vector3 _targetPoint;
    protected bool _isPatrolling;
    protected bool _isChasing;
    protected bool _isAttacking;
    protected Vector3 _moveDir;
    protected float _moveSpeed;
    protected Rigidbody _rb;
    protected float _attackRange;
    protected float _chaseRange;

    private void Start()
    {
        _manager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        if (_manager != null)
        {
            _attackRange = _manager._attackRange;
            _chaseRange = _manager._chaseRange;
            _manager._enemies.Add(this);
            _playerController = _manager._playerController;
        }
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        _isPatrolling = true;
        _isChasing = false;
        _isAttacking = false;
        _hasAttacked = false;

        _originalObject = gameObject;

        _rb = gameObject.GetComponent<Rigidbody>();

        ActivatePatrol();
    }

    protected float GetPlayerDistance()
    {
        Vector3 start = transform.position;
        Vector3 end = _playerController.transform.position;
        float diffX = end.x - start.x;
        float diffY = end.y - start.y;
        float diffZ = end.z - start.z;
        return (float)Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ);
    }

    protected Vector3 GetPlayerDirection()
    {
        return (_targetPoint - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        //check if the player is on sight and if it's in the attack range.
        _rb.AddForce(_moveDir * _moveSpeed, ForceMode.Force);

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
            transform.LookAt(_playerController.transform.position);

        if (_isChasing)
        {
            //Debug.Log("CHASING");
            _moveDir = GetPlayerDirection();

            if (GetPlayerDistance() <= _attackRange)
            {
                _isChasing = false;
                _isAttacking = true;
            }
        }
        else if (_isPatrolling)
        {
            //Debug.Log("PATROLLING");
            transform.LookAt(_targetPoint);
        }
        else if (_isAttacking)
        {
            //Debug.Log("ATTACKING");
            Attack();
        }
    }

    private void Update()
    {
        if (_hasAttacked)
            _timeSinceAttacked += Time.deltaTime;
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
