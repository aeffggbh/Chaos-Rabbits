using UnityEngine;

public class NormalEnemy : Enemy
{
    private ClownAnimationController clownAnimation;

    protected override void Start()
    {
        base.Start();
        animationController = gameObject.AddComponent<ClownAnimationController>();
        clownAnimation = animationController as ClownAnimationController;
    }

    protected override void ActivatePatrol()
    {
        base.ActivatePatrol();

        if (animationController != null)
            clownAnimation.Walk();
        else
            Debug.LogError("AnimationController is null for " + gameObject.name);
    }
    protected override void ActivateChase()
    {
        _moveSpeed = _chasingSpeed;
        _moveDir = GetPlayerDirection();
    }

    protected override void Attack()
    {
        _rb.linearVelocity = Vector3.zero;
        if (_moveSpeed > 0)
        {
            _moveSpeed = 0;
            clownAnimation.StopWalking();
        }
        if ((_manager._attackTimer - _timeSinceAttacked) < 1)
        {
            _timeSinceAttacked = 0;
            //stay still and shoot the player until it's out of range.
        }
    }
    protected override void Move()
    {
        if (_moveSpeed > 0)
            clownAnimation.Walk();
        else
            clownAnimation.StopWalking();
        //se mueve normal
        //Debug.Log("speed: " + _moveSpeed);  
        _rb.AddForce((_moveDir * _moveSpeed /*+ _counterMovement*/) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    protected override void ActivateIdle()
    {
        _rb.linearVelocity = Vector3.zero;

        if (animationController != null)
            clownAnimation.StopWalking();
        else
            Debug.LogError("AnimationController is null for " + gameObject.name);
    }

    protected override void ActivateAttack()
    {
        throw new System.NotImplementedException();
    }
}
