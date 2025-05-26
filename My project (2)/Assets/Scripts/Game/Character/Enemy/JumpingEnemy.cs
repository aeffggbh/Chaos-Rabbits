using UnityEngine;

/// <summary>
/// Represents an enemy that jumps towards the player.
/// </summary>
public class JumpingEnemy : Enemy
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

        _jumpForce = GameManager.savedPlayer.GetJumpForce() / 2;
        _currentJumpForce = _jumpForce;
        _defaultMoveSpeed = _moveSpeed;
        //no conviene que sea tan rapido
        _patrolSpeed /= 2;
        _chasingSpeed /= 2;
    }


    protected override void ActivateChase()
    {
        _moveSpeed = _chasingSpeed;
    }

    protected override void Attack()
    {
        if (_timeSinceAttacked > _manager.attackTimer)
        {
            _timeSinceAttacked = 0;
            JumpAtPlayer();
        }
    }

    protected override void Move()
    {
        if (IsGrounded())
        {
            if (_currentJumpForce > _jumpForce * _jumpForceMultiplier)
            {
                ResetJumpForce();
                ResetMoveSpeed();
            }

            Vector3 velocity = _rb.linearVelocity;
            velocity.y = _currentJumpForce;
            _rb.linearVelocity = velocity;
        }

        _rb.AddForce((_moveDir * _moveSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse); 

    }

    private bool IsGrounded()
    {
        if (Time.time > _timer + _rate)
        {
            _timer = Time.time;
            Vector3 origin = transform.position + Vector3.up * 0.1f;
            return Physics.Raycast(origin, Vector3.down, _groundCheckDistance);
        }
        return false;
    }

    protected override void ActivateIdle()
    {
        ResetJumpForce();
    }

    protected override void ActivateAttack()
    {
        _moveSpeed = _chasingSpeed;
    }

    private void JumpAtPlayer()
    {
        _currentJumpForce = _jumpForce * _jumpForceMultiplier;
        _moveSpeed = _defaultMoveSpeed * _speedMultiplier;
    }

    public override void Die()
    {
        base.Die();
        DeleteCharacterObject();
    }

    private void ResetJumpForce()
    {
        _currentJumpForce = _jumpForce;
    }

    private void ResetMoveSpeed()
    {
        _moveSpeed = _defaultMoveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
                player.TakeDamage(damage);
        }
    }
}
