using UnityEngine;

/// <summary>
/// Represents a normal enemy that shoots the player
/// </summary>
public class ShooterEnemy : Enemy, IPatrolBehavior, IChaseBehavior, IAttackBehavior, IMovementBehavior, IIdleBehavior, IWeaponUser
{
    private ShooterAnimationController _clownAnimation;
    private Weapon _enemyWeapon;

    public Weapon CurrentWeapon { get => _enemyWeapon; set { _enemyWeapon = value; } }

    public GameObject UserObject => gameObject;

    private void OnEnable()
    {
        _enemyWeapon = GetComponent<WeaponHolder>().currentWeapon;
        _enemyWeapon.user = this;
    }

    protected override void Start()
    {
        base.Start();
        animationController = gameObject.AddComponent<ShooterAnimationController>();
        _clownAnimation = animationController as ShooterAnimationController;
    }

    /// <summary>
    /// Activates the patrol behavior and plays the walk animation.
    /// </summary>
    public void ActivatePatrol()
    {
        if (animationController != null)
            _clownAnimation.AnimateWalk();
    }

    /// <summary>
    /// Activates the chase behavior and sets the movement direction towards the player.
    /// </summary>
    public void ActivateChase()
    {
        _currentSpeed = _chasingSpeed;
        _moveDir = GetPlayerDirection();
    }

    /// <summary>
    /// Performs the attack action, stopping movement and firing the weapon if possible.
    /// </summary>
    public void Attack()
    {
        Rb.linearVelocity = Vector3.zero;
        if (_currentSpeed > 0)
        {
            _currentSpeed = 0;
            if (_clownAnimation != null)
                _clownAnimation.AnimateStopWalking();
        }
        if (_timeSinceAttacked > _manager.AttackTimer)
        {
            _timeSinceAttacked = 0;
            _enemyWeapon.Fire();
        }
    }

    /// <summary>
    /// Handles the movement logic and updates the animation based on speed.
    /// </summary>
    public void Move()
    {
        if (_clownAnimation != null)
        {
            if (_currentSpeed > 0)
                _clownAnimation.AnimateWalk();
            else
                _clownAnimation.AnimateStopWalking();
        }

        Vector3 force = _moveDir * CurrentSpeed + _counterMovement;
        Rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    /// <summary>
    /// Activates the idle behavior, stopping movement and animations.
    /// </summary>
    public void ActivateIdle()
    {
        Rb.linearVelocity = Vector3.zero;

        if (animationController != null)
            _clownAnimation.AnimateStopWalking();
    }
}
