using UnityEngine;

public class NormalEnemy : Enemy
{
    protected override void ActivatePatrol()
    {
        Debug.Log("yeah");
        float randomZ = UnityEngine.Random.Range(-_manager._walkRange, _manager._walkRange);
        float randomX = UnityEngine.Random.Range(-_manager._walkRange, _manager._walkRange);

        _targetPoint = new Vector3(transform.position.x + randomX,
                                     transform.position.y,
                                     transform.position.z + randomZ);

        _moveDir = (_targetPoint - transform.position).normalized;
        _moveSpeed = _playerController.GetWalkSpeed();

        _isPatrolling = true;
    }

    protected override void ActivateChase()
    {
        Debug.Log("yeah");
        _moveSpeed = _playerController.GetRunSpeed();
        _moveDir = GetPlayerDirection();
    }

    protected override void Attack()
    {
        Debug.Log("yeah");
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
