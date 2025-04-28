using System;
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
    [SerializeField] private float _speed;
    [SerializeField] private float _force;
    [SerializeField] private float _counterMovementForce;
    [SerializeField] private float _jumpForce;
    [SerializeField] private CinemachineBrain _cineMachineBrain;
    private Camera _cineMachineCamera;
    private Vector3 _counterMovement;
    private Vector2 _moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _3DMovement;
    private ForceRequest _forceRequest;

    private void OnEnable()
    {
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



        if (!_cineMachineBrain)
            Debug.LogError(nameof(_cineMachineBrain) + " is null");
        else
            _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();


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

            // character.RequestSelfRotation(_cineMachineBrain.transform.eulerAngles.x);

        }
        else
            _3DMovement = new Vector3(_moveInput.x, 0, _moveInput.y);

        if (_character)
            _character.RequestMovement(_3DMovement);
        else
            Debug.LogError(nameof(_character) + " is null");
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
            speed = _speed,
            acceleration = _force,
            counterMovement = _counterMovement,
            counterMovementForce = _counterMovementForce,
            forceMode = ForceMode.Impulse
        };

        if (_character)
            _character.RequestForce(_forceRequest);
        else
            Debug.LogError(nameof(_character) + " is null");

    }


}