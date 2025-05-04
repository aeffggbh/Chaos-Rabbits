using UnityEngine;

public class NormalEnemy : Enemy
{

    protected override void ActivatePatrol()
    {
        _patrolCurrentTime = 0;
        float randomZ = UnityEngine.Random.Range(-_manager._walkRange, _manager._walkRange);
        float randomX = UnityEngine.Random.Range(-_manager._walkRange, _manager._walkRange);

        _targetWalk = new Vector3(transform.position.x + randomX,
                                     transform.position.y,
                                     transform.position.z + randomZ);

        _targetLook = _targetWalk*2;


        _moveDir = (_targetWalk - transform.position).normalized;
        _moveDir.y = 0;
        _moveSpeed = _patrolSpeed;

        if (!_isPatrolling)
            _isPatrolling = true;
    }

    protected override void ActivateChase()
    {
        _moveSpeed = _chasingSpeed;
        _moveDir = GetPlayerDirection();
    }

    protected override void Attack()
    {
        if (GetPlayerDistance() > _attackRange)
        {
            _isChasing = true;
            _isAttacking = false;
        }

        if (_isAttacking)
        {
            if (!_hasAttacked)
                _hasAttacked = true;

            if (_timeSinceAttacked == _manager._attackTimer)
            {
                _timeSinceAttacked = 0;
                //stay still and shoot the player until it's out of range.
                if (_moveSpeed > 0)
                    _moveSpeed = 0;
            }
        }
    }
}
