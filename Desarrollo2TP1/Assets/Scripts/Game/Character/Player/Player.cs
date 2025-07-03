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
public class Player : Character
{
    private IPlayerMovement _movement;
    private IPlayerJump _jump;
    private IPlayerWeaponHandler _weaponHandler;
    private ISoundPlayer _soundPlayer;

    public IPlayerMovement Movement { get { return _movement; } }

    private Rigidbody _rb;
    [Header("Character")]
    [SerializeField] private float _damage;
    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _counterMovementForce;
    private Vector2 currentDirection;
    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [Header("Weapon")]
    [SerializeField] public Weapon currentWeapon;
    [SerializeField] private Transform _weaponParent;
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    private IPlayerMovementCalculator _playerCalculator;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        IsWeaponUser = true;

        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        Damage = _damage;
    }

    protected override void Start()
    {
        base.Start();

        _movement = new PlayerMovement(_rb, _runSpeed, _walkSpeed, _acceleration, _counterMovementForce);
        _soundPlayer = new SoundPlayer(_audioSource);
        _jump = new PlayerJump(_rb, _soundPlayer);
        _playerCalculator = new PlayerMovementCalculator();
        _soundPlayer.SetAudioSource(GetComponent<AudioSource>());

    }

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

    public void RequestWeaponGrab(Weapon weapon)
    {
        _weaponHandler.GrabWeapon(weapon);
    }

    public void RequestWeaponDrop()
    {
        _weaponHandler.DropWeapon();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 calculatedMovement = _playerCalculator.GetDirection(currentDirection);
        _movement.Move(calculatedMovement, _playerCalculator);
        
        _jump.Jump(_jumpForce);
    }

    public override void Die()
    {
        if (!CheatsController.Instance.IsGodMode())
            SceneController.GoToScene(SceneController.GameState.GAMEOVER);
    }

    public override void TakeDamage(float damage)
    {
        if (!CheatsController.Instance.IsGodMode())
            base.TakeDamage(damage);
        else
            Debug.Log("Cannot take damage!");
    }

    public float GetJumpForce()
    {
        return _jumpForce;
    }

}
