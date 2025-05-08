using UnityEngine;

public class NormalEnemy : Enemy
{
  
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

            if (_moveSpeed > 0)
                _moveSpeed = 0;
            if ((_manager._attackTimer - _timeSinceAttacked) < 1)
            {
                _timeSinceAttacked = 0;
                //stay still and shoot the player until it's out of range.
            }
        }
    }
    protected override void Move()
    {
        //se mueve normal
        _rb.AddForce((_moveDir * _moveSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

}
