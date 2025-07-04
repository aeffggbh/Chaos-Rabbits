using UnityEngine;

/// <summary>
/// Represents a normal enemy that shoots the player
/// </summary>
public class NormalEnemy : Enemy, IPatrolBehavior, IChaseBehavior, IAttackBehavior, IMovementBehavior, IIdleBehavior, IWeaponUser
{
    private ClownAnimationController _clownAnimation;
    private Weapon _enemyWeapon;

    public Weapon CurrentWeapon { get => _enemyWeapon; set { _enemyWeapon = value; } }

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

    public void ActivatePatrol()
    {
        if (animationController != null)
            _clownAnimation.Walk();
    }
    public void ActivateChase()
    {
        _moveSpeed = _chasingSpeed;
        _moveDir = GetPlayerDirection();
    }

    public void Attack()
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
    public void Move()
    {
        if (_moveSpeed > 0)
            _clownAnimation.Walk();
        else
            _clownAnimation.StopWalking();

        _rb.AddForce((_moveDir * _moveSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    public void ActivateIdle()
    {
        _rb.linearVelocity = Vector3.zero;

        if (animationController != null)
            _clownAnimation.StopWalking();
        else
            Debug.LogError("AnimationController is null for " + gameObject.name);
    }

    public void ActivateAttack()
    {
        Debug.Log(nameof(NormalEnemy) + " is about to attack");
    }
}
