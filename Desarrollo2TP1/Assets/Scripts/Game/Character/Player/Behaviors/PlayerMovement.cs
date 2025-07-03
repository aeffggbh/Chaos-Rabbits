
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.InputSystem;

public class PlayerMovement : IPlayerMovement
{
    private readonly Rigidbody _rb;
    private float _currentSpeed;
    private float _acceleration;
    private float _counterMovementForce;
    private float _runSpeed;
    private float _walkSpeed;

    public float CurrentSpeed
    {
        get { return _currentSpeed; }
        set { _currentSpeed = value; }
    }

    public PlayerMovement(Rigidbody rb, float runSpeed, float walkSpeed, float acceleration, float counterMovementForce)
    {
        _rb = rb;
        _currentSpeed = walkSpeed;
        _acceleration = acceleration;
        _counterMovementForce = counterMovementForce;
        _runSpeed = runSpeed;
        _walkSpeed = walkSpeed;
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Vector3 counterMovement = Vector3.zero;
        counterMovement.x = -_rb.linearVelocity.x * _counterMovementForce;
        counterMovement.z = -_rb.linearVelocity.z * _counterMovementForce;

        Vector3 force = (direction * _currentSpeed + counterMovement) * _acceleration;
        _rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
    }


    public void Move(InputAction.CallbackContext context, IPlayerMovementCalculator calculator, 
        PlayerAnimationController animation, Player player)
    {
        animation.Walk();

        CheckFlash();
        Vector3 movementDirection = calculator.GetDirection(context.ReadValue<Vector2>());
        player.RequestMovementDirection(movementDirection);
    }

    public void StopMoving(Player player, PlayerAnimationController animation)
    {
        _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);

        player.RequestMovementDirection(new(0, _rb.linearVelocity.y, 0));
        animation.StopWalking();
    }

    private void CheckFlash()
    {
        if (CheatsController.Instance.IsFlashMode())
            _currentSpeed = _runSpeed;
        else if (_currentSpeed == _runSpeed)
            _currentSpeed = _runSpeed;
    }
}
