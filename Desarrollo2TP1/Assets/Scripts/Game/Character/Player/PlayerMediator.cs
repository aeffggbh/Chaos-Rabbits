using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: poner que no se puedan deseleccionar los botones para que no se joda con el joystick.
//TODO: ponerle summary a todo lo que no lo tiene
//TODO: NO PUEDEN HABER COMENTARIOS QUE NO SEAN SUMMARIES.
// NO PUEDE HABER NADA EN ESPAÑOL
// No pueden haber errores de ortografia (ver si hay configs para eso)
// Ponerle un asset a la bala... No la pelotita :3
// saca los find
// 7/7 se entrega uwu

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


    [SerializeField] private CinemachineBrain _cineMachineBrain;
    [SerializeField] private GameObject _bulletSpawn;
    [SerializeField] private float _maxWeaponDistance;
    [SerializeField] private float _grabDropCooldown;

    [SerializeField] public Player player;
    [SerializeField] private PlayerAnimationController _playerAnimation;
    private IPlayerMovementCalculator _playerMovement;
    private IPlayerWeaponHandler _playerWeapon;
    private IPlayerInputHandler _playerInput;
    private IPlayerSceneHandler _playerScene;

    private void Awake()
    {
        ServiceProvider.SetService<PlayerMediator>(this, true);
        _playerAnimation = GetComponent<PlayerAnimationController>();
        player = GetComponent<Player>();

        _playerMovement = new PlayerMovementCalculator();

        _playerWeapon = new PlayerWeaponHandler(
            _bulletSpawn,
            _maxWeaponDistance,
            _grabDropCooldown,
            player.currentWeapon,
            _playerAnimation
            );

        _playerInput = new PlayerInputHandler();

        _playerScene = new PlayerSceneHandler(_cineMachineBrain);


        if (_maxWeaponDistance < 1)
            Debug.LogWarning("Distance to weapon is too low!");

        if (_grabDropCooldown <= 0)
            Debug.LogWarning("The cooldown when grabbing and dropping the weapon is too low. This may cause glitches");
    }

    private void Start()
    {
        InitCamera();

        if (!_bulletSpawn)
            Debug.LogError(nameof(_bulletSpawn) + " is null");


    }

    private void InitCamera()
    {
        if (!_cineMachineBrain)
            _cineMachineBrain = CineMachineManager.Instance.GetComponent<CinemachineBrain>();

        if (_cineMachineBrain)
            _playerMovement.SetCamera(_cineMachineBrain.GetComponent<Camera>());
        else
            Debug.LogError(nameof(CinemachineBrain) + " is null");
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnDestroy()
    {
        _playerInput.Disable();
        ServiceProvider.SetService<PlayerMediator>(null);
    }

    private void Update()
    {
        _playerScene.CheckPlayerDestroy();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
            player?.RequestGroundedState(true);
    }

    /// <summary>
    /// Applies damage to the player character when it collides with an enemy or takes damage from a weapon.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        player.TakeDamage(damage);
    }

    /// <summary>
    /// Destroys the player and the cinemachinebrain (because they are not destroyed on load)
    /// </summary>
    public void Destroy()
    {
        Destroy(_cineMachineBrain.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// Grabs a weapon if the player is pointing to it and is not already holding one.
    /// </summary>
    /// <param name="context"></param>
    public void OnGrabWeapon(InputAction.CallbackContext context)
    {
        if (GameManager.paused)
            return;

        _playerWeapon.GrabPointedWeapon(_playerMovement.Camera);
    }

    /// <summary>
    /// Drops the currently held weapon if the player is holding one.
    /// </summary>
    /// <param name="context"></param>
    public void OnDropWeapon(InputAction.CallbackContext context)
    {
        if (GameManager.paused)
            return;

        _playerWeapon.DropWeapon();
    }

    /// <summary>
    /// Handles the jump action when the player presses the jump button.
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        player.RequestJumpInfo(true);
    }

    /// <summary>
    /// Handles the cancellation of movement when the player releases the movement input.
    /// </summary>
    /// <param name="context"></param>
    public void OnCancelMove(InputAction.CallbackContext context)
    {
        player.Movement.StopMoving(player, _playerAnimation);
    }

    /// <summary>
    /// Handles the movement input from the player and applies the corresponding force to the player character.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        player.Movement.Move(context, _playerMovement, _playerAnimation, player);
    }
}