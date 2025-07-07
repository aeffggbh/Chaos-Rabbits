using System;
using UnityEngine;

/// <summary>
/// Represents an enemy that jumps towards the player.
/// </summary>
public class JumpingEnemy : Enemy, IMovementBehavior, IChaseBehavior, IAttackBehavior, IIdleBehavior
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpForceMultiplier = 3.5f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private float _groundCheckDistance = 0.3f;

    private float _defaultMoveSpeed;
    private float _currentJumpForce;
    private float _timer;
    private float _rate = 0.5f;

    /// <summary>
    /// Initializes the enemy and sets up initial values.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        _jumpForce = PlayerMediator.PlayerInstance.Player.GetJumpForce() / 2;
        _currentJumpForce = _jumpForce;
        _defaultMoveSpeed = _currentSpeed;
        _patrolSpeed /= 2;
        _chasingSpeed /= 2;
    }

    /// <summary>
    /// Activates the chase behavior by setting the current speed to chasing speed.
    /// </summary>
    public void ActivateChase()
    {
        _currentSpeed = _chasingSpeed;
    }

    /// <summary>
    /// Triggers the attack behavior and boosts movement if attack cooldown has passed.
    /// </summary>
    public void Attack()
    {
        if (_timeSinceAttacked > _manager.AttackTimer)
        {
            _timeSinceAttacked = 0;
            BoostMovement();
        }
    }

    /// <summary>
    /// Handles the movement logic, including jumping if grounded.
    /// </summary>
    public void Move()
    {
        if (IsGrounded())
        {
            if (_currentJumpForce > _jumpForce * _jumpForceMultiplier)
            {
                ResetJumpForce();
                ResetMoveSpeed();
            }

            ActivateJump();
        }

        Rb.AddForce((_moveDir * _currentSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);

    }

    /// <summary>
    /// Applies a jump force to the enemy's rigidbody.
    /// </summary>
    private void ActivateJump()
    {
        Vector3 velocity = Rb.linearVelocity;
        velocity.y = _currentJumpForce;
        Rb.linearVelocity = velocity;
    }

    /// <summary>
    /// Checks if the enemy is currently grounded using a raycast.
    /// </summary>
    private bool IsGrounded()
    {
        if (Time.time > _timer + _rate)
        {
            _timer = Time.time;
            return RayManager.IsGrounded(transform, _groundCheckDistance);
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
        _currentSpeed = _chasingSpeed;
    }

    /// <summary>
    /// Boosts the enemy's jump force and movement speed for an attack.
    /// </summary>
    private void BoostMovement()
    {
        _currentJumpForce = _jumpForce * _jumpForceMultiplier;
        _currentSpeed = _defaultMoveSpeed * _speedMultiplier;
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
        _currentSpeed = _defaultMoveSpeed;
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
