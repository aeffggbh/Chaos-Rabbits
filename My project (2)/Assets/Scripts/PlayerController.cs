using System;
using System.Drawing;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _force;
    [SerializeField] private float _counterMovementForce;
    [SerializeField] private float _jumpForce;
    [SerializeField] private CinemachineBrain _cineMachineBrain;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private GameObject _head;
    [SerializeField] private float _weaponBoxIncrease;
    [SerializeField] private float _maxWeaponDistance;
    private Ray _rayFront;
    private bool _holdingWeapon;
    private Camera _cineMachineCamera;
    private Vector2 _moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _3DMovement;
    private ForceRequest _forceRequest;

    private void OnEnable()
    {
        _weaponBoxIncrease = 1.2f;

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

        _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();

    }

    private void GrabWeapon(InputAction.CallbackContext context)
    {
        if (PointingToWeapon())
        {
            _holdingWeapon = true;
            _weapon.Hold();
        }
    }

    private void DropWeapon(InputAction.CallbackContext context)
    {
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
        //d = √ [(x2 – x1)2 + (y2 – y1)2 + (z2 – z1)2].
        Vector3 start = transform.position;
        Vector3 end = _weapon.transform.position;
        float diffX = end.x - start.x;
        float diffY = end.y - start.y;
        float diffZ = end.z - start.z;
        float distance = (float)Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ);

        Vector3 pointInView = _rayFront.origin + (_rayFront.direction * distance);

        BoxCollider boxCollider = _weapon.GetComponent<BoxCollider>();

        if (boxCollider == null) return false;

        Vector3 max = boxCollider.bounds.max;
        Vector3 min = boxCollider.bounds.min;

        max.x += _weaponBoxIncrease;
        max.y += _weaponBoxIncrease;
        max.z += _weaponBoxIncrease;
        min.x -= _weaponBoxIncrease;
        min.y -= _weaponBoxIncrease;
        min.z -= _weaponBoxIncrease;

        //Debug.Log("Min: " + min);
        //Debug.Log("Max: " + max);
        Debug.Log("Distance: " + distance);

        return (pointInView.x >= min.x && pointInView.x <= max.x &&
                pointInView.y >= min.y && pointInView.y <= max.y &&
                pointInView.z >= min.z && pointInView.z <= max.z)
                &&
                !_holdingWeapon
                &&
                distance <= _maxWeaponDistance;

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

    public Transform GetCinemachineCamera()
    {
        return _cineMachineCamera.transform;
    }

    public GameObject GetHead()
    {
        return _head;
    }
}