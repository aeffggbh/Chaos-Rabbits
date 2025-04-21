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
    [SerializeField] private Character character;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private float _speed;
    [SerializeField] private float _force;
    [SerializeField] private float _counterMovementForce;
    [SerializeField] private float _jumpForce;
    [SerializeField] private CinemachineBrain _cineMachineBrain;
    private Camera _cineMachineCamera;
    private Vector3 _counterMovement;
    private Vector2 moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _3DMovement;
    private ForceRequest forceRequest;

    private void OnEnable()
    {
        if (moveAction == null)
            return;

        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnCancelMove;

        if (jumpAction == null)
            return;

        jumpAction.action.started += OnJump;

        moveInput = new Vector2(0, 0);

        if (_cineMachineBrain == null)
            return;

        _cineMachineCamera = _cineMachineBrain.GetComponent<Camera>();

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        character.RequestJumpInfo(true, _jumpForce);
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMove;
    }

    private void OnCollisionEnter(Collision other)
    {
        //TODO: verify it's actually the floor
        if (other != null)
        {
            character.RequestGroundedState(true);
        }
    }

    private void FixedUpdate()
    {
        if (_cineMachineBrain != null)
        {
            if (moveInput.x != 0 || moveInput.y != 0)
            {
                _camForward = _cineMachineCamera.transform.forward;
                _camRight = _cineMachineCamera.transform.right;

                _3DMovement = _camForward * moveInput.y + _camRight * moveInput.x;

                _3DMovement.y = 0;
            }
            else
                _3DMovement = Vector3.zero;

            // character.RequestSelfRotation(_cineMachineBrain.transform.eulerAngles.x);

        }
        else
            _3DMovement = new Vector3(moveInput.x, 0, moveInput.y);

        character.RequestMovement(_3DMovement);
    }

    private void OnCancelMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        forceRequest.direction = moveInput;
        character.RequestForce(forceRequest);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        forceRequest = new ForceRequest
        {
            direction = _3DMovement,
            speed = _speed,
            acceleration = _force,
            counterMovement = _counterMovement,
            counterMovementForce = _counterMovementForce,
            forceMode = ForceMode.Impulse
        };

        character.RequestForce(forceRequest);
    }


}