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

    public InputActionReference MoveAction { get { return _moveAction; } set { _moveAction = value; } }
    public InputActionReference JumpAction { get { return _jumpAction; } set { _jumpAction = value; } }
    public InputActionReference DropAction { get { return _dropAction; } set { _dropAction = value; } }
    public InputActionReference GrabAction { get { return _grabAction; } set { _grabAction = value; } }


    [SerializeField] private GameObject _bulletSpawnGO;
    [SerializeField] private float _maxWeaponDistance;
    [SerializeField] private float _grabDropCooldown;

    [SerializeField] private Player _player;
    [SerializeField] private PlayerAnimationController _playerAnimation;
    [SerializeField] private Transform _weaponParent;
    private IPlayerMovementCalculator _playerMovement;
    private IPlayerWeaponHandler _playerWeapon;
    private IPlayerInputEnabler _playerInput;

    private static PlayerMediator _instance;

    public Player Player { get => _player; set => _player = value; }
    public static PlayerMediator PlayerInstance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _playerInput = new PlayerInputEnabler();
    }

    private void Start()
    {

        _playerAnimation = GetComponent<PlayerAnimationController>();
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

        _playerWeapon = new PlayerWeaponHandler(
            _bulletSpawnGO,
            _maxWeaponDistance,
            _grabDropCooldown,
            _player.CurrentWeapon,
            _playerAnimation,
            _weaponParent
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
        if (_playerInput != null)
            _playerInput.Disable();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
            _player?.RequestGroundedState(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
            _player?.RequestGroundedState(false);
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
    /// Destroys the player and the cinemachinebrain (because they are not destroyed on load)
    /// </summary>
    public void Destroy()
    {
        if (CineMachineManager.Instance.cineMachineBrain && gameObject)
        {
            Destroy(CineMachineManager.Instance.cineMachineBrain.gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Grabs a weapon if the player is pointing to it and is not already holding one.
    /// </summary>
    /// <param name="context"></param>
    public void OnGrabWeapon(InputAction.CallbackContext context)
    {
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
        _player.RequestJumpInfo(true);
    }

    /// <summary>
    /// Handles the cancellation of movement when the player releases the movement input.
    /// </summary>
    /// <param name="context"></param>
    public void OnCancelMove(InputAction.CallbackContext context)
    {
        _player.Movement.StopMoving(_player, _playerAnimation);
    }

    /// <summary>
    /// Handles the movement input from the player and applies the corresponding force to the player character.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _player.Movement.RequestMovement(context, _playerMovement, _playerAnimation, _player);
    }

}