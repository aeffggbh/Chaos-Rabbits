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
        _currentSpeed = _chasingSpeed;
        _moveDir = GetPlayerDirection();
    }

    public void Attack()
    {
        Rb.linearVelocity = Vector3.zero;
        if (_currentSpeed > 0)
        {
            _currentSpeed = 0;
            if (_clownAnimation != null)
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
        if (_clownAnimation != null)
        {
            if (_currentSpeed > 0)
                _clownAnimation.Walk();
            else
                _clownAnimation.StopWalking();
        }

        Rb.AddForce((_moveDir * _currentSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    public void ActivateIdle()
    {
        Rb.linearVelocity = Vector3.zero;

        if (animationController != null)
            _clownAnimation.StopWalking();
    }

    public void ActivateAttack()
    {
        Debug.Log(nameof(NormalEnemy) + " is about to attack");
    }
}
