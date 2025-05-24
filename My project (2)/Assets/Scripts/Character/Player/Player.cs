using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    private ForceRequest _forceRequest;
    private Rigidbody rb;
    private bool _isJumping;
    private float _jumpForce;
    private bool _grounded;
    private Vector3 _calculatedMovement;
    private bool _isGodMode;

    private void Awake()
    {
        maxHealth = 100.0f;
        currentHealth = maxHealth;

        IsWeaponUser = true;
        Obj = gameObject;

        _forceRequest = null;
        _jumpForce = 0f;
        _isJumping = false;
        _grounded = true;

        rb = GetComponent<Rigidbody>();
    }

    public void RequestGodMode(bool godMode)
    {
        _isGodMode = godMode;
    }

    public void RequestForce(ForceRequest forceRequest)
    {
        //instantForceRequests.Add(forceRequest);
        _forceRequest = forceRequest;
    }

    public void RequestJumpInfo(bool isJumping, float jumpForce)
    {
        _isJumping = isJumping;
        _jumpForce = jumpForce;
    }

    public void RequestGroundedState(bool grounded)
    {
        _grounded = grounded;
    }

    public void RequestMovement(Vector3 calculatedMovement)
    {
        _calculatedMovement = calculatedMovement;
    }

    private void FixedUpdate()
    {
        //MOVEMENT
        {
            if (_forceRequest != null)
            {
                if (_forceRequest.direction != _calculatedMovement)
                    _forceRequest.direction = _calculatedMovement;

                if (_forceRequest.forceMode == ForceMode.Impulse)
                {
                    _forceRequest._counterMovement = new Vector3
                        (-rb.linearVelocity.x * _forceRequest._counterMovementForce,
                        0,
                        -rb.linearVelocity.z * _forceRequest._counterMovementForce);

                    rb.AddForce((_forceRequest.direction * _forceRequest.speed + _forceRequest._counterMovement) * Time.fixedDeltaTime,
                                ForceMode.Impulse);
                }
            }

        }

        //JUMPING
        {
            if (_isJumping)
            {
                if (_grounded)
                    Jump();

                _isJumping = false;
            }
        }
    }


    private void Jump()
    {
        _grounded = false;
        rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }


    public override void Die()
    {
        if (!_isGodMode)
            //TODO: implement "You Died" screen
            Debug.Log("YOU DIED");
    }

    public override void TakeDamage(float damage)
    {
        if (!_isGodMode)
            base.TakeDamage(damage);
        else
            Debug.Log("Cannot take damage!");
    }

}
