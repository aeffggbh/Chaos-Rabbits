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
    
    protected override void Start()
    {
        base.Start();

        _jumpForce = PlayerMediator.PlayerInstance.Player.GetJumpForce() / 2;
        _currentJumpForce = _jumpForce;
        _defaultMoveSpeed = _currentSpeed;
        _patrolSpeed /= 2;
        _chasingSpeed /= 2;
    }

    public void ActivateChase()
    {
        _currentSpeed = _chasingSpeed;
    }

    public void Attack()
    {
        if (_timeSinceAttacked > _manager.attackTimer)
        {
            _timeSinceAttacked = 0;
            BoostMovement();
        }
    }

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

    private void ActivateJump()
    {
        Vector3 velocity = Rb.linearVelocity;
        velocity.y = _currentJumpForce;
        Rb.linearVelocity = velocity;
    }

    private bool IsGrounded()
    {
        if (Time.time > _timer + _rate)
        {
            _timer = Time.time;
            return RayManager.IsGrounded(transform, _groundCheckDistance);
        }
        return false;
    }

    public void ActivateIdle()
    {
        ResetJumpForce();
    }

    public void ActivateAttack()
    {
        _currentSpeed = _chasingSpeed;
    }

    private void BoostMovement()
    {
        _currentJumpForce = _jumpForce * _jumpForceMultiplier;
        _currentSpeed = _defaultMoveSpeed * _speedMultiplier;
    }

    private void ResetJumpForce()
    {
        _currentJumpForce = _jumpForce;
    }

    private void ResetMoveSpeed()
    {
        _currentSpeed = _defaultMoveSpeed;
    }

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
