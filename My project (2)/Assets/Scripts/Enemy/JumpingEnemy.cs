using UnityEngine;

public class JumpingEnemy : Enemy
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float groundCheckDistance = 0.3f;
    private float timer;
    private float rate = 0.5f;

    protected override void Start()
    {
        base.Start();

        _jumpForce = _playerController.GetJumpForce() / 2;
        //  _rb.constraints = RigidbodyConstraints.FreezeRotationX
        //            | RigidbodyConstraints.FreezeRotationZ;
    }


    protected override void ActivateChase()
    {
    }

    protected override void Attack()
    {
        _moveSpeed = _chasingSpeed;
    }

    protected override void Move()
    {

        if (IsGrounded())
        {
            //_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            Vector3 velocity = _rb.linearVelocity;
            velocity.y = _jumpForce;
            _rb.linearVelocity = velocity;
        }


        //// 2) Apply horizontal “bunny‐hop” movement
        //Vector3 horizontalForce = _moveDir * _moveSpeed;
        //_rb.AddForce(horizontalForce, ForceMode.Acceleration);
        _rb.AddForce((_moveDir * _moveSpeed) * Time.fixedDeltaTime, ForceMode.Impulse);

        // 3) Clamp max horizontal speed so we never flip backward
        //Vector3 v = _rb.linearVelocity;
        //Vector3 horiz = new Vector3(v.x, 0, v.z);
        //if (horiz.magnitude > _moveSpeed)
        //{
        //    horiz = horiz.normalized * _moveSpeed;
        //    _rb.linearVelocity = new Vector3(horiz.x, v.y, horiz.z);
        //}

    }

    private bool IsGrounded()
    {
        if (Time.time > timer + rate)
        {
            timer = Time.time;
            Vector3 origin = transform.position + Vector3.up * 0.1f;
            return Physics.Raycast(origin, Vector3.down, groundCheckDistance);
        }
        return false;
    }

    protected override void ActivateIdle()
    {
    }

    protected override void ActivateAttack()
    {
        throw new System.NotImplementedException();
    }
}
