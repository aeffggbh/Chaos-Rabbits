using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reads input and decides actions taken by the player
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerMediator : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _dropAction;
    [SerializeField] private InputActionReference _grabAction;
    [SerializeField] private InputActionReference _lookAction;

    public InputActionReference MoveAction => _moveAction;
    public InputActionReference JumpAction => _jumpAction;
    public InputActionReference DropAction => _dropAction;
    public InputActionReference GrabAction => _grabAction;
    public InputActionReference LookAction => _lookAction;

    [SerializeField] private GameObject _bulletSpawnGO;
    [SerializeField] private float _maxWeaponDistance;
    [SerializeField] private float _grabDropCooldown;

    [SerializeField] private Player _player;
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private GameObject _fallbackWeaponPrefab;
    [SerializeField] private CheatsController _cheatsController;
    [SerializeField] private CinemachineCamera _camera;
    private IPlayerMovementCalculator _playerMovement;
    private IPlayerWeaponHandler _playerWeapon;
    private IPlayerInputEnabler _playerInput;

    public Player Player { get => _player; set => _player = value; }
    public CheatsController CheatsController => _cheatsController;
    public CinemachineCamera Camera => _camera;

    private void Awake()
    {
        ServiceProvider.SetService<PlayerMediator>(this, true);

        _camera = GetComponentInChildren<CinemachineCamera>();
        _playerInput = new PlayerInputEnabler();
    }

    private void Start()
    {
        _player = GetComponent<Player>();

        _playerMovement = new PlayerMovementCalculator();

        if (_bulletSpawnGO == null)
        {
            Debug.LogWarning($"No bullet spawn found for {gameObject.name}");
        }

        if (_weaponParent == null)
        {
            Debug.LogWarning($"No parent for weapons");
        }

        if (_fallbackWeaponPrefab == null)
            Debug.LogWarning($"No default weapon");

        _playerWeapon = new PlayerWeaponHandler(
            _bulletSpawnGO,
            _maxWeaponDistance,
            _player.PlayerAnimation,
            _weaponParent,
            _fallbackWeaponPrefab,
            _player.SavedWeaponData
            );

        if (_maxWeaponDistance < 1)
            Debug.LogWarning("Distance to weapon is too low!");

        if (_grabDropCooldown <= 0)
            Debug.LogWarning("The cooldown when grabbing and dropping the weapon is too low. This may cause glitches");
        if (!_bulletSpawnGO)
            Debug.LogError(nameof(_bulletSpawnGO) + " is null");
    }

    private void OnEnable()
    {
        _playerInput?.Enable();
    }

    private void OnDisable()
    {
        _playerInput?.Disable();
    }

    private void OnDestroy()
    {
        _playerInput?.Disable();
    }

    /// <summary>
    /// Applies damage to the player character when it collides with an enemy or takes damage from a weapon.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        _player.TakeDamage(damage);
    }

    /// <summary>
    /// Grabs a weapon if the player is pointing to it and is not already holding one.
    /// </summary>
    /// <param name="context"></param>
    public void OnGrabWeapon(InputAction.CallbackContext context)
    {
        if (!this)
            return;

        if (PauseManager.Paused)
            return;

        _playerWeapon.GrabPointedWeapon(_playerMovement.Camera);
    }

    /// <summary>
    /// Drops the currently held weapon if the player is holding one.
    /// </summary>
    /// <param name="context"></param>
    public void OnDropWeapon(InputAction.CallbackContext context)
    {
        if (!this)
            return;

        if (PauseManager.Paused)
            return;

        _playerWeapon.DropWeapon();
    }

    /// <summary>
    /// Handles the jump action when the player presses the jump button.
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!this)
            return;

        _player.Jump();
    }

    /// <summary>
    /// Handles the cancellation of movement when the player releases the movement input.
    /// </summary>
    /// <param name="context"></param>
    public void OnCancelMove(InputAction.CallbackContext context)
    {
        if (!this)
            return;

        _player?.Movement?.StopMoving(_player);
    }

    /// <summary>
    /// Handles the movement input from the player and applies the corresponding force to the player character.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!this)
            return;

        _player?.Movement?.RequestMovement(context, _playerMovement, _player);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!this)
            return;

        if (_player)
            EventTriggerer.Trigger<IPlayerLookEvent>(new PlayerLookEvent(gameObject, context));
    }
}