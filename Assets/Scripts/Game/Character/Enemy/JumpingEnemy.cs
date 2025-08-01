using System;
using UnityEngine;

/// <summary>
/// Represents an enemy that jumps towards the player.
/// </summary>
[RequireComponent(typeof(RabbitAnimationController))]
public class JumpingEnemy : Enemy, IMovementBehavior, IChaseBehavior, IAttackBehavior, IAttackActivationBehavior, IIdleBehavior
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpForceMultiplier = 1.5f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private float _groundCheckDistance = 0.3f;
    [SerializeField] private RabbitAnimationController _rabbitAnimation;

    BoxCollider _collider;

    private float _defaultMoveSpeed;
    private float _currentJumpForce;
    private float _timer;
    private float _rate = 0.5f;

    protected override void Start()
    {
        base.Start();

        _jumpForce = 6f;

        _currentJumpForce = _jumpForce;
        _defaultMoveSpeed = currentSpeed;

        patrolSpeed /= 2;
        chasingSpeed /= 2;

        _rabbitAnimation = GetComponent<RabbitAnimationController>();

        animationController = _rabbitAnimation;

        _collider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Activates the chase behavior by setting the current speed to chasing speed.
    /// </summary>
    public void ActivateChase()
    {
        currentSpeed = chasingSpeed;
    }

    /// <summary>
    /// Triggers the attack behavior and boosts movement if attack cooldown has passed.
    /// </summary>
    public void Attack()
    {
        if (timeSinceAttacked > manager.AttackTimer)
        {
            timeSinceAttacked = 0;
            BoostMovement();
        }
    }

    /// <summary>
    /// Handles the movement logic, including jumping if grounded.
    /// </summary>
    public void Move()
    {
        if (CanJump())
        {
            if (_currentJumpForce > _jumpForce * _jumpForceMultiplier)
            {
                ResetJumpForce();
                ResetMoveSpeed();
            }

            ActivateJump();
        }

        if (!_rabbitAnimation.IsLanding())
            Rb.AddForce((moveDir * currentSpeed + counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    /// <summary>
    /// Applies a jump force to the enemy's rigidbody.
    /// </summary>
    private void ActivateJump()
    {
        Vector3 velocity = Rb.linearVelocity;
        velocity.y = _currentJumpForce;
        Rb.linearVelocity = velocity;

        _rabbitAnimation.TriggerJump();
    }

    /// <summary>
    /// Checks if the enemy is currently grounded using a raycast.
    /// </summary>
    private bool CanJump()
    {
        bool isGrounded = RayManager.IsGrounded(_collider, 0.1f);

        _rabbitAnimation.UpdateGround(isGrounded);

        bool isLanding = _rabbitAnimation.IsLanding();

        if (Time.time > _timer + _rate &&
            !isLanding)
        {
            _timer = Time.time;

            return isGrounded;
        }
        return false;
    }

    /// <summary>
    /// Activates the idle behavior and resets jump force.
    /// </summary>
    public void ActivateIdle()
    {
        ResetJumpForce();
    }

    /// <summary>
    /// Activates the attack state by setting the current speed to chasing speed.
    /// </summary>
    public void ActivateAttack()
    {
        currentSpeed = chasingSpeed;
    }

    /// <summary>
    /// Boosts the enemy's jump force and movement speed for an attack.
    /// </summary>
    private void BoostMovement()
    {
        _currentJumpForce = _jumpForce * _jumpForceMultiplier;
        currentSpeed = _defaultMoveSpeed * _speedMultiplier;
    }

    /// <summary>
    /// Resets the jump force to its default value.
    /// </summary>
    private void ResetJumpForce()
    {
        _currentJumpForce = _jumpForce;
    }

    /// <summary>
    /// Resets the movement speed to its default value.
    /// </summary>
    private void ResetMoveSpeed()
    {
        currentSpeed = _defaultMoveSpeed;
    }

    /// <summary>
    /// Handles collision with the player and applies damage.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMediator player = collision.gameObject.GetComponent<PlayerMediator>();
            if (player != null)
                player.TakeDamage(Damage);
        }
    }
}
