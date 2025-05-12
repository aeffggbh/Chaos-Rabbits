using UnityEngine;

public class JumpingEnemy : Enemy
{
    float _jumpForce;
    bool _isJumping;

    protected override void Start()
    {
        base.Start();

        _jumpForce = _playerController.GetJumpForce();
        _isJumping = true;
    }


    protected override void ActivateChase()
    {
    }

    protected override void Attack()
    {
    }

    protected override void Move()
    {
        if (_isJumping)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isJumping = false;
        }

        _rb.AddForce((_moveDir * _moveSpeed + _counterMovement) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
            _isJumping = true;
    }
}
