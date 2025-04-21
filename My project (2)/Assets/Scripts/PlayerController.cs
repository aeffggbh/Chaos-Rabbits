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
        if (_moveAction == null)
            return;

        _moveAction.action.performed += OnMove;
        _moveAction.action.canceled += OnCancelMove;

        if (_jumpAction == null)
            return;

        _jumpAction.action.started += OnJump;

        _moveInput = new Vector2(0, 0);

        if (_cineMachineBrain == null)
            return;

        _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _character.RequestJumpInfo(true, _jumpForce);
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= OnMove;
    }

    private void OnCollisionEnter(Collision other)
    {
        //TODO: verify it's actually the floor
        if (other != null)
        {
            _character.RequestGroundedState(true);
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

        _character.RequestMovement(_3DMovement);
    }

    private void OnCancelMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _forceRequest.direction = _moveInput;
        _character.RequestForce(_forceRequest);
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

        _character.RequestForce(_forceRequest);
    }


}