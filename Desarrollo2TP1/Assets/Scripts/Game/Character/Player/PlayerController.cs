using Unity.Cinemachine;
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
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _dropAction;
    [SerializeField] private InputActionReference _grabAction;
    [SerializeField] private CinemachineBrain _cineMachineBrain;
    [SerializeField] private GameObject _bulletSpawn;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _force;
    [SerializeField] private float _counterMovementForce;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxWeaponDistance;
    [SerializeField] private float _grabDropCooldown;
    [SerializeField] private float _damage;
    [SerializeField] public Weapon currentWeapon;
    private float _currentSpeed;
    private Ray _rayForward;
    private Camera _cineMachineCamera;
    private Vector2 _moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _3DMovement;
    private ForceRequest _forceRequest;
    private bool _holdingWeapon;
    private AudioSource _audioSource;
    private SoundManager _soundManager;
    private PlayerAnimationController _playerAnimation;

    private void Awake()
    {
        ServiceProvider.SetService<PlayerController>(this, true);
        _playerAnimation = GetComponent<PlayerAnimationController>();
    }

    private void OnDestroy()
    {
        if (_moveAction)
        {
            _moveAction.action.performed -= OnMove;
            _moveAction.action.canceled -= OnCancelMove;
        }
        if (_jumpAction)
            _jumpAction.action.started -= OnJump;
        if (_dropAction)
            _dropAction.action.started -= DropWeapon;
        if (_grabAction)
            _grabAction.action.started -= GrabWeapon;

        ServiceProvider.SetService<PlayerController>(null);
    }

    private void OnEnable()
    {
        _player = GetComponent<Player>();

        _holdingWeapon = false;

        _moveInput = new Vector2(0, 0);

        if (_damage < 0.1f)
            Debug.Log("Damage was not defined or it's too low");
        else
            _player.damage = _damage;

        if (!_moveAction)
            Debug.LogError(nameof(_moveAction) + " is null");
        else
        {
            _moveAction.action.performed += OnMove;
            _moveAction.action.canceled += OnCancelMove;
        }

        if (!_jumpAction)
            Debug.LogError(nameof(_jumpAction) + " is null");
        else
            _jumpAction.action.started += OnJump;


        if (!_dropAction)
            Debug.LogError(nameof(_dropAction) + "is null");
        else
            _dropAction.action.started += DropWeapon;

        if (!_grabAction)
            Debug.LogError(nameof(_grabAction) + "is null");
        else
            _grabAction.action.started += GrabWeapon;

        if (_maxWeaponDistance < 1)
            Debug.LogWarning("Distance to weapon is too low!");

        if (_grabDropCooldown <= 0)
            Debug.LogWarning("The cooldown when grabbing and dropping the weapon is too low. This may cause glitches");

        _currentSpeed = _walkSpeed;
    }

    private void Start()
    {
        if (!currentWeapon)
            Debug.LogWarning("Player has no weapon!");
        else
            currentWeapon.Hold();

        _cineMachineBrain = CinemachineManager.instance.GetComponent<CinemachineBrain>();

        if (_cineMachineBrain)
            _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();
        else
            Debug.Log(nameof(_cineMachineBrain) + " is null");

        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;

        _audioSource = GetComponent<AudioSource>();

        _player.RequestSound(_soundManager, _audioSource);

        if (!_bulletSpawn)
            Debug.LogError(nameof(_bulletSpawn) + " is null");
    }

    /// <summary>
    /// Grabs a weapon if the player is pointing to it and is not already holding one.
    /// </summary>
    /// <param name="context"></param>
    private void GrabWeapon(InputAction.CallbackContext context)
    {
        if (!GameManager.paused)
        {
            Weapon weaponToGrab = GetPointedWeapon();

            if (weaponToGrab)
            {
                if (_holdingWeapon)
                    currentWeapon.Drop();

                _holdingWeapon = true;
                currentWeapon = weaponToGrab;
                currentWeapon.SetUser(_player, typeof(Player));
                currentWeapon.SetBulletSpawn(_bulletSpawn);
                currentWeapon.user = _player;
                currentWeapon.Hold();
                _playerAnimation.GrabWeapon();
            }
        }
    }

    /// <summary>
    /// Drops the currently held weapon if the player is holding one.
    /// </summary>
    /// <param name="context"></param>
    private void DropWeapon(InputAction.CallbackContext context)
    {
        if (!GameManager.paused)
            if (_holdingWeapon)
            {
                _holdingWeapon = false;
                currentWeapon.Drop();
                _playerAnimation.DropWeapon();
            }
    }

    /// <summary>
    /// Returns the weapon that the player is currently pointing to, if any.
    /// </summary>
    /// <returns></returns>
    private Weapon GetPointedWeapon()
    {
        RaycastHit? hit = null; // Use a nullable RaycastHit

        if (RayManager.PointingToObject(_cineMachineCamera.transform, 50f, out RaycastHit hitInfo))
            hit = hitInfo;

        if (hit != null)
        {
            Weapon weapon = hit.Value.collider.gameObject.GetComponent<Weapon>();

            if (weapon)
                return weapon;
        }

        return null;

    }

    /// <summary>
    /// Handles the jump action when the player presses the jump button.
    /// </summary>
    /// <param name="context"></param>
    private void OnJump(InputAction.CallbackContext context)
    {
        _player.RequestJumpInfo(true, _jumpForce);
    }

    private void OnDisable()
    {
        if (_moveAction)
            _moveAction.action.performed -= OnMove;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            if (_player)
                _player.RequestGroundedState(true);
            else
                Debug.LogError(nameof(_player) + " is null");
        }
    }

    private void Update()
    {
        CheckExistence();

        if (_cineMachineBrain)
        {
            if (_rayForward.direction != _cineMachineCamera.transform.forward)
                _rayForward.direction = _cineMachineCamera.transform.forward;

            if (_rayForward.origin != _cineMachineCamera.transform.position)
                _rayForward.origin = _cineMachineCamera.transform.position;
        }
        else
            _cineMachineBrain = CinemachineManager.instance.GetComponent<CinemachineBrain>();

    }

    /// <summary>
    /// Checks if the player is in a valid scene and destroys the player if not.
    /// </summary>
    private void CheckExistence()
    {
        SceneController.CheckCurrentScene();

        if (!SceneController.IsGameplay(SceneController.currentScene))
            DestroyPlayer();
    }

    private void FixedUpdate()
    {
        if (_cineMachineBrain != null)
        {
            if (_moveInput.x != 0 || _moveInput.y != 0)
            {
                _camForward = _cineMachineCamera.transform.forward;
                _camRight = _cineMachineCamera.transform.right;

                _3DMovement = _camForward * _moveInput.y + _camRight * _moveInput.x;

                _3DMovement.y = 0;
            }
            else
                _3DMovement = Vector3.zero;

        }
        else
            _3DMovement = new Vector3(_moveInput.x, 0, _moveInput.y);

        if (_player)
            _player.RequestMovement(_3DMovement);
        else
            Debug.LogError(nameof(_player) + " is null");

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawRay(_rayForward);
    }

    /// <summary>
    /// Handles the cancellation of movement when the player releases the movement input.
    /// </summary>
    /// <param name="context"></param>
    private void OnCancelMove(InputAction.CallbackContext context)
    {
        _playerAnimation.StopWalking();

        _moveInput = context.ReadValue<Vector2>();
        if (_forceRequest != null)
            _forceRequest.direction = _moveInput;
        else
            Debug.LogError(nameof(_forceRequest) + " is null");

        if (_player)
            _player.RequestForce(_forceRequest);
        else
            Debug.LogWarning(nameof(_player) + " is null");

    }

    /// <summary>
    /// Handles the movement input from the player and applies the corresponding force to the player character.
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        _playerAnimation.Walk();

        _moveInput = context.ReadValue<Vector2>();

        if (CheatsController.instance.IsFlashMode())
            _currentSpeed = _runSpeed;
        else if (_currentSpeed == _runSpeed)
            _currentSpeed = _walkSpeed;

        _forceRequest = new ForceRequest
        {
            direction = _3DMovement,
            speed = _currentSpeed,
            acceleration = _force,
            counterMovementForce = _counterMovementForce,
            forceMode = ForceMode.Impulse
        };

        if (_player)
            _player.RequestForce(_forceRequest);
        else
            Debug.LogError(nameof(_player) + " is null");

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
    /// Returns the jump force of the player character.
    /// </summary>
    /// <returns></returns>
    public float GetJumpForce()
    {
        return _jumpForce;
    }

    /// <summary>
    /// Destroys the player and the cinemachinebrain (because they are not destroyed on load)
    /// </summary>
    public void DestroyPlayer()
    {
        Destroy(_cineMachineBrain.gameObject);
        Destroy(gameObject);
    }

}