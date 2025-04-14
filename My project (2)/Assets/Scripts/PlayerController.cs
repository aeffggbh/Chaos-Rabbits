using System;
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
    [SerializeField] private float _speed;
    [SerializeField] private float _force;

    private void OnEnable()
    {
        if (moveAction == null)
            return;

        moveAction.action.performed += OnMove;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var horizontalInput = context.ReadValue<Vector2>();
        var forceRequest = new ForceRequest
        {
            direction = new Vector3(horizontalInput.x, 0, horizontalInput.y),
            speed = _speed,
            acceleration = _force
        };

        character.RequestContinuousForce(forceRequest);
    }
}