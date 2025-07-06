using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : IPlayerMovement
{
    Vector3 counterMovement = Vector3.zero;

    IPhysicsMovementData _data;

    public float CurrentSpeed { get => _data.CurrentSpeed; set => _data.CurrentSpeed = value; }

    public PlayerMovement(IPhysicsMovementData data)
    {
        _data = data;

        if (_data.CurrentSpeed == 0)
            _data.CurrentSpeed = _data.WalkSpeed;
    }

    /// <summary>
    /// Moves the player to a certain direction
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="calculator"></param>
    public void Move(Vector3 direction, IPlayerMovementCalculator calculator)
    {
        if (direction == Vector3.zero)
            return;

        counterMovement.x = -_data.Rb.linearVelocity.x * _data.CounterMovementForce;
        counterMovement.z = -_data.Rb.linearVelocity.z * _data.CounterMovementForce;

        Vector3 force = (direction * _data.CurrentSpeed + counterMovement) * _data.Acceleration;
        _data.Rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    /// <summary>
    /// Reads the player input for the movement
    /// </summary>
    /// <param name="context"></param>
    /// <param name="calculator"></param>
    /// <param name="animation"></param>
    /// <param name="player"></param>
    public void Move(InputAction.CallbackContext context, IPlayerMovementCalculator calculator,
        PlayerAnimationController animation, IPlayerData player)
    {
        animation.Walk();

        CheckFlash();

        (player as Player)?.RequestMovementDirection(context.ReadValue<Vector2>());
    }

    public void StopMoving(IPlayerData player, PlayerAnimationController animation)
    {
        _data.Rb.linearVelocity = new Vector3(0, _data.Rb.linearVelocity.y, 0);

        (player as Player)?.RequestMovementDirection(Vector2.zero);
        animation.StopWalking();
    }

    private void CheckFlash()
    {
        if (CheatsController.Instance.IsFlashMode())
            _data.CurrentSpeed = _data.RunSpeed;
        else if (_data.CurrentSpeed == _data.RunSpeed)
            _data.CurrentSpeed = _data.WalkSpeed;
    }
}
