using UnityEngine;

/// <summary>
/// Represents a normal enemy that shoots the player
/// </summary>
public class NormalEnemy : Enemy
{
    private ClownAnimationController _clownAnimation;
    private Weapon _enemyWeapon;

    private void OnEnable()
    {
        IsWeaponUser = true;
        _enemyWeapon = GetComponentInChildren<WeaponHolder>().currentWeapon;
        _enemyWeapon.user = this;
    }

    protected override void Start()
    {
        base.Start();
        animationController = gameObject.AddComponent<ClownAnimationController>();
        _clownAnimation = animationController as ClownAnimationController;
    }

    protected override void ActivatePatrol()
    {
        base.ActivatePatrol();

        if (animationController != null)
            _clownAnimation.Walk();
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
            _clownAnimation.StopWalking();
        }
        if (_timeSinceAttacked > _manager.attackTimer)
        {
            _timeSinceAttacked = 0;
            _enemyWeapon.Fire();
        }
    }
    protected override void Move()
    {
        if (_moveSpeed > 0)
            _clownAnimation.Walk();
        else
            _clownAnimation.StopWalking();

        _rb.AddForce((_moveDir * _moveSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    protected override void ActivateIdle()
    {
        _rb.linearVelocity = Vector3.zero;

        if (animationController != null)
            _clownAnimation.StopWalking();
        else
            Debug.LogError("AnimationController is null for " + gameObject.name);
    }

    protected override void ActivateAttack()
    {
    }

    public override void Die()
    {
        base.Die();
        DeleteCharacterObject();
    }
}
