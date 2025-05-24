using System;
using System.Drawing;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;
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
    [SerializeField] private CheatsController _cheatsController;
    [SerializeField] private CinemachineBrain _cineMachineBrain;
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private GameObject _head;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _force;
    [SerializeField] private float _counterMovementForce;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxWeaponDistance;
    [SerializeField] private float _grabDropCooldown;
    [SerializeField] private float _damage;
    private float _currentSpeed;
    private float _lastGrabDropTime;
    private Ray _rayFront;
    private Camera _cineMachineCamera;
    private Vector2 _moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _3DMovement;
    private ForceRequest _forceRequest;
    private bool _holdingWeapon;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
            _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();

        if (!_head)
            Debug.LogError("Player has no head!");

        if (!_currentWeapon)
            Debug.LogError("Player has no weapon!");

        if (_maxWeaponDistance < 1)
            Debug.LogWarning("Distance to weapon is too low!");

        if (_grabDropCooldown <= 0)
            Debug.LogWarning("The cooldown when grabbing and dropping the weapon is too low. This may cause glitches");

        if (!_weaponManager)
            Debug.LogError(nameof(_weaponManager) + " is null");

        if (!_cheatsController)
            Debug.LogError(nameof(_cheatsController) + " is null");

        _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();
        _currentSpeed = _walkSpeed;
    }

    private bool CheckGrabDropCooldown()
    {
        return Time.time - _lastGrabDropTime < _grabDropCooldown;
    }

    private void GrabWeapon(InputAction.CallbackContext context)
    {
        if (CheckGrabDropCooldown())
            return;

        Weapon weaponToGrab = PointedWeapon();

        if (weaponToGrab)
        {
            if (_holdingWeapon)
                _currentWeapon.Drop();

            _holdingWeapon = true;
            _currentWeapon = weaponToGrab;
            _currentWeapon.SetUser(_player, typeof(Player));
            _currentWeapon.user = _player;
            _currentWeapon.Hold();
        }
    }

    private void DropWeapon(InputAction.CallbackContext context)
    {
        if (CheckGrabDropCooldown())
            return;

        if (_holdingWeapon)
        {
            _holdingWeapon = false;
            _currentWeapon.Drop();
        }
    }

    private Weapon PointedWeapon()
    {
        for (int i = 0; i < _weaponManager.weapons.Count; i++)
            if (_weaponManager.weapons[i] != null)
                if (PointingToWeapon(_weaponManager.weapons[i]))
                    return _weaponManager.weapons[i];

        return null;
    }

    private bool PointingToWeapon(Weapon weapon)
    {
        RayManager _pointDetection = new();

        return _pointDetection.PointingToObject(_cineMachineCamera.transform, weapon.transform, weapon.GetComponent<Collider>()) &&
                !_holdingWeapon &&
                _pointDetection.GetDistanceToObject() <= _maxWeaponDistance;

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_player != null)
        {
            _player.RequestJumpInfo(true, _jumpForce);
        }
        else
            Debug.LogError(nameof(_player) + " is null");
    }

    private void OnDisable()
    {
        if (_moveAction)
            _moveAction.action.performed -= OnMove;
    }

    private void OnCollisionEnter(Collision other)
    {
        //TODO: verify it's actually the floor lol
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
        if (_rayFront.direction != _cineMachineCamera.transform.forward)
            _rayFront.direction = _cineMachineCamera.transform.forward;

        if (_rayFront.origin != _cineMachineCamera.transform.position)
            _rayFront.origin = _cineMachineCamera.transform.position;
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

    private void GodModeCheck()
    {
        _player.RequestGodMode(_cheatsController._isGodMode);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawRay(_rayFront);
    }

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
            Debug.LogError(nameof(_player) + " is null");

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (_cheatsController._isFlashMode)
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

    public void TakeDamage(float damage)
    {
        _player.TakeDamage(damage);
    }

    public float GetWalkSpeed()
    {
        return _walkSpeed;
    }

    public float GetRunSpeed()
    {
        return _runSpeed;
    }

    public float GetJumpForce()
    {
        return _jumpForce;
    }

    public Transform GetCinemachineCamera()
    {
        return _cineMachineCamera.transform;
    }

    public GameObject GetHead()
    {
        return _head;
    }
}