using UnityEngine;

/// <summary>
/// Represents the player character in the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    private ForceRequest _forceRequest;
    private Rigidbody rb;
    private bool _isJumping;
    private float _jumpForce;
    private bool _grounded;
    private Vector3 _calculatedMovement;
    private SoundManager _soundManager;
    private AudioSource _audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        IsWeaponUser = true;

        _forceRequest = null;
        _jumpForce = 0f;
        _isJumping = false;
        _grounded = true;

        rb = GetComponent<Rigidbody>();
    }

    public void RequestSound(SoundManager soundManager, AudioSource audioSource)
    {
        _soundManager = soundManager;
        _audioSource = audioSource;
    }

    /// <summary>
    /// Requests a force to be applied to the player character.
    /// </summary>
    /// <param name="forceRequest"></param>
    public void RequestForce(ForceRequest forceRequest)
    {
        //instantForceRequests.Add(forceRequest);
        _forceRequest = forceRequest;
    }

    /// <summary>
    /// Requests jump information for the player character.
    /// </summary>
    /// <param name="isJumping"></param>
    /// <param name="jumpForce"></param>
    public void RequestJumpInfo(bool isJumping, float jumpForce)
    {
        _isJumping = isJumping;
        _jumpForce = jumpForce;
    }

    /// <summary>
    /// Requests the grounded state of the player character.
    /// </summary>
    /// <param name="grounded"></param>
    public void RequestGroundedState(bool grounded)
    {
        _grounded = grounded;
    }

    /// <summary>
    /// Requests movement for the player character based on calculated movement vector.
    /// </summary>
    /// <param name="calculatedMovement"></param>
    public void RequestMovement(Vector3 calculatedMovement)
    {
        _calculatedMovement = calculatedMovement;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

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

    /// <summary>
    /// Applies an impulse force to the player character to make it jump.
    /// </summary>
    private void Jump()
    {
        _soundManager.PlaySound(SoundType.JUMP, _audioSource);
        _grounded = false;
        rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }


    public override void Die()
    {
        if (!CheatsController.instance.IsGodMode())
            SceneController.GoToScene(SceneController.GameState.GAMEOVER);
    }

    public override void TakeDamage(float damage)
    {
        if (!CheatsController.instance.IsGodMode())
            base.TakeDamage(damage);
        else
            Debug.Log("Cannot take damage!");
    }

}
