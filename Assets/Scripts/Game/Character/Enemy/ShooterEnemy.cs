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
        currentSpeed = chasingSpeed;
        moveDir = GetPlayerDirection();
    }

    /// <summary>
    /// Performs the attack action, stopping movement and firing the weapon if possible.
    /// </summary>
    public void Attack()
    {
        rb.linearVelocity = Vector3.zero;
        if (currentSpeed > 0)
        {
            currentSpeed = 0;
            if (_clownAnimation != null)
                _clownAnimation.AnimateStopWalking();
        }
        if (timeSinceAttacked > stats.AttackTimer)
        {
            timeSinceAttacked = 0;
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
            if (currentSpeed > 0)
                _clownAnimation.AnimateWalk();
            else
                _clownAnimation.AnimateStopWalking();
        }

        Vector3 force = moveDir * currentSpeed + counterMovement;
        rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    /// <summary>
    /// Activates the idle behavior, stopping movement and animations.
    /// </summary>
    public void ActivateIdle()
    {
        rb.linearVelocity = Vector3.zero;

        if (animationController != null)
            _clownAnimation.AnimateStopWalking();
    }
}
