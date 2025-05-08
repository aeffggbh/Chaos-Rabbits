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
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _dropAction;
    [SerializeField] private InputActionReference _grabAction;
    [SerializeField] private CinemachineBrain _cineMachineBrain;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private GameObject _head;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _force;
    [SerializeField] private float _counterMovementForce;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxWeaponDistance;
    [SerializeField] private float _grabDropCooldown;
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

        _holdingWeapon = true;

        _moveInput = new Vector2(0, 0);
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

        if (!_weapon)
            Debug.LogError("Player has no weapon!");

        if (_maxWeaponDistance < 1)
            Debug.LogWarning("Distance to weapon is too low!");

        if (_grabDropCooldown <= 0)
            Debug.LogWarning("The cooldown when grabbing and dropping the weapon is too low. This may cause glitches");

        _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();

    }

    private bool CheckGrabDropCooldown()
    {
        return Time.time - _lastGrabDropTime < _grabDropCooldown;
    }

    private void GrabWeapon(InputAction.CallbackContext context)
    {
        if (CheckGrabDropCooldown()) return;

        if (PointingToWeapon())
        {
            _holdingWeapon = true;
            _weapon.Hold();
        }
    }

    private void DropWeapon(InputAction.CallbackContext context)
    {
        if (CheckGrabDropCooldown()) return;

        if (_holdingWeapon)
        {
            _holdingWeapon = false;
            _weapon.Drop();
        }
    }

    /// <summary>
    /// https://unacademy.com/content/nda/study-material/mathematics/analytical-geometry-three-dimensions-distance-between-two-points/#:~:text=Similarly%2C%20to%20calculate%20the%20distance,between%20two%20points%20is%20required.&text=PQ%20%3D%20d%20%3D%20%E2%88%9A%20%5B(x,%E2%80%93%20z1)2%5D.
    /// </summary>
    private bool PointingToWeapon()
    {
        RayManager _pointDetection = new();

        return _pointDetection.PointingToObject(_cineMachineCamera.transform, _weapon.transform, _weapon.GetComponent<BoxCollider>()) &&
                !_holdingWeapon &&
                _pointDetection.GetDistanceToObject() <= _maxWeaponDistance;

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_character != null)
        {
            _character.RequestJumpInfo(true, _jumpForce);
        }
        else
            Debug.LogError(nameof(_character) + " is null");
    }

    private void OnDisable()
    {
        if (_moveAction)
            _moveAction.action.performed -= OnMove;
    }

    private void OnCollisionEnter(Collision other)
    {
        //TODO: verify it's actually the floor lol
        if (other != null)
        {
            if (_character)
                _character.RequestGroundedState(true);
            else
                Debug.LogError(nameof(_character) + " is null");
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

        if (_character)
            _character.RequestMovement(_3DMovement);
        else
            Debug.LogError(nameof(_character) + " is null");

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

        if (_character)
            _character.RequestForce(_forceRequest);
        else
            Debug.LogError(nameof(_character) + " is null");

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        _forceRequest = new ForceRequest
        {
            direction = _3DMovement,
            speed = _walkSpeed,
            acceleration = _force,
            _counterMovementForce = _counterMovementForce,
            forceMode = ForceMode.Impulse
        };

        if (_character)
            _character.RequestForce(_forceRequest);
        else
            Debug.LogError(nameof(_character) + " is null");

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