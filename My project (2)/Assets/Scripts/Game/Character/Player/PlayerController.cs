using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reads input and decides actions taken by the player
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _dropAction;
    [SerializeField] private InputActionReference _grabAction;
    [SerializeField] private CinemachineBrain _cineMachineBrain;
    [SerializeField] private GameObject _head;
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
    private Ray _rayFront;
    private Camera _cineMachineCamera;
    private Vector2 _moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _3DMovement;
    private ForceRequest _forceRequest;
    private bool _holdingWeapon;

    private void Awake()
    {
        ServiceProvider.SetService<PlayerController>(this, true);
    }

    private void OnEnable()
    {
        _player = GetComponent<Player>();

        _holdingWeapon = true;

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


        if (!_cineMachineBrain)
            Debug.LogError(nameof(_cineMachineBrain) + " is null");
        else
        {
            _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();
            DontDestroyOnLoad(_cineMachineBrain);
        }

        if (!_head)
            Debug.LogError("Player has no head!");

        if (!currentWeapon)
            Debug.LogError("Player has no weapon!");

        if (_maxWeaponDistance < 1)
            Debug.LogWarning("Distance to weapon is too low!");

        if (_grabDropCooldown <= 0)
            Debug.LogWarning("The cooldown when grabbing and dropping the weapon is too low. This may cause glitches");

        _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();
        _currentSpeed = _walkSpeed;
    }

    /// <summary>
    /// Grabs a weapon if the player is pointing to it and is not already holding one.
    /// </summary>
    /// <param name="context"></param>
    private void GrabWeapon(InputAction.CallbackContext context)
    {
        if (!GameManager.paused)
        {
            Weapon weaponToGrab = PointedWeapon();

            if (weaponToGrab)
            {
                if (_holdingWeapon)
                    currentWeapon.Drop();

                _holdingWeapon = true;
                currentWeapon = weaponToGrab;
                currentWeapon.SetUser(_player, typeof(Player));
                currentWeapon.user = _player;
                currentWeapon.Hold();
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
            }
    }

    /// <summary>
    /// Returns the weapon that the player is currently pointing to, if any.
    /// </summary>
    /// <returns></returns>
    private Weapon PointedWeapon()
    {
        for (int i = 0; i < WeaponManager.instance.weapons.Count; i++)
            if (WeaponManager.instance.weapons[i] != null)
                if (PointingToWeapon(WeaponManager.instance.weapons[i]))
                    return WeaponManager.instance.weapons[i];

        return null;
    }

    /// <summary>
    /// Checks if the player is pointing to a specific weapon within a certain distance.
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    private bool PointingToWeapon(Weapon weapon)
    {
        RayManager _pointDetection = new();

        return _pointDetection.PointingToObject(_cineMachineCamera.transform, weapon.transform, weapon.GetComponent<Collider>()) &&
                !_holdingWeapon &&
                _pointDetection.GetDistanceToObject() <= _maxWeaponDistance;

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
            if (_rayFront.direction != _cineMachineCamera.transform.forward)
                _rayFront.direction = _cineMachineCamera.transform.forward;

            if (_rayFront.origin != _cineMachineCamera.transform.position)
                _rayFront.origin = _cineMachineCamera.transform.position;
        }
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
        GodModeCheck();

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

    /// <summary>
    /// Checks if the player is in god mode and applies it if so.
    /// </summary>
    private void GodModeCheck()
    {
        _player.RequestGodMode(GameManager.cheatsController._isGodMode);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawRay(_rayFront);
    }

    /// <summary>
    /// Handles the cancellation of movement when the player releases the movement input.
    /// </summary>
    /// <param name="context"></param>
    private void OnCancelMove(InputAction.CallbackContext context)
    {
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
        _moveInput = context.ReadValue<Vector2>();

        if (GameManager.cheatsController._isFlashMode)
            _currentSpeed = _runSpeed;
        else if (_currentSpeed == _runSpeed)
            _currentSpeed = _walkSpeed;

        _forceRequest = new ForceRequest
        {
            direction = _3DMovement,
            speed = _currentSpeed,
            acceleration = _force,
            _counterMovementForce = _counterMovementForce,
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
        Destroy(_cineMachineBrain);
        Destroy(gameObject);
    }

    /// <summary>
    /// Resets the player's health to its maximum value
    /// </summary>
    public void ResetPlayer()
    {
        _player.currentHealth = _player.maxHealth;
    }
}