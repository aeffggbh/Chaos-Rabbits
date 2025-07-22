using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents the player character in the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(PlayerAnimationController))]
public class Player : Character, IPlayerData
{
    private IPlayerMovement _movement;
    private IPlayerJump _jump;
    private IPlayerWeaponHandler _weaponHandler;
    private ISoundPlayer _soundPlayer;

    /// <summary>
    /// saves the data of the player movement
    /// </summary>
    public IPlayerMovement Movement { get { return _movement; } }

    private Rigidbody _rb;
    [Header("Character")]
    [SerializeField] private float _damage;
    [Header("Movement")]
    private float _currentSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _counterMovementForce;
    private Vector2 currentDirection;
    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [Header("Weapon")]
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private Transform _weaponParent;
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    private IPlayerMovementCalculator _playerCalculator;

    public Weapon CurrentWeapon { get => _currentWeapon; set { _currentWeapon = value; } }
    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public float CounterMovementForce { get => _counterMovementForce; set => _counterMovementForce = value; }
    public float RunSpeed { get => _runSpeed; set => _runSpeed = value; }
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; }
    public Rigidbody Rb { get => _rb; set => _rb = value; }
    public Transform WeaponParent { get => _weaponParent; set => _weaponParent = value; }
    public GameObject UserObject => gameObject;

    private void Awake()
    {

        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        Damage = _damage;
    }

    protected override void Start()
    {
        base.Start();

        _movement = new PlayerMovement(this);
        _soundPlayer = new SoundPlayer(_audioSource);
        _jump = new PlayerJump(_rb, _soundPlayer);
        _playerCalculator = new PlayerMovementCalculator();
        _soundPlayer.SetAudioSource(GetComponent<AudioSource>());

    }

    /// <summary>
    /// Requests the movement direction and saves it
    /// </summary>
    /// <param name="direction"></param>
    public void RequestMovementDirection(Vector2 direction)
    {
        currentDirection = direction;
    }

    /// <summary>
    /// Requests jump information for the player character.
    /// </summary>
    /// <param name="shouldJump"></param>
    /// <param name="jumpForce"></param>
    public void RequestJumpInfo(bool shouldJump)
    {
        _jump.SetJumpState(shouldJump);
    }

    /// <summary>
    /// Requests the grounded state of the player character.
    /// </summary>
    /// <param name="grounded"></param>
    public void RequestGroundedState(bool grounded)
    {
        _jump.IsGrounded = grounded;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 calculatedMovement = _playerCalculator.GetDirection(currentDirection);
        _movement.Move(calculatedMovement);

        _jump.Jump(_jumpForce);
    }

    public override void Die()
    {
        if (!CheatsController.Instance.IsGodMode())
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameOverState(), gameObject));
    }

    public override void TakeDamage(float damage)
    {
        if (!CheatsController.Instance.IsGodMode())
            base.TakeDamage(damage);
        else
            Debug.Log("Cannot take damage!");
    }

    /// <summary>
    /// Returns the jump force
    /// </summary>
    /// <returns></returns>
    public float GetJumpForce()
    {
        return _jumpForce;
    }

}
