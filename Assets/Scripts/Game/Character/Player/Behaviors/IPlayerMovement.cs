using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interface for handling the movement of the player
/// </summary>
public interface IPlayerMovement
{
    /// <summary>
    /// Saves the current speed of the player
    /// </summary>
    float CurrentSpeed { get; }
    /// <summary>
    /// Moves the player given a direction
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="calculator"></param>
    void Move(Vector3 direction);
    /// <summary>
    /// Request information necessary for the movement of the player
    /// </summary>
    /// <param name="context"></param>
    /// <param name="calculator"></param>
    /// <param name="animation"></param>
    /// <param name="player"></param>
    void RequestMovement(InputAction.CallbackContext context, IPlayerMovementCalculator calculator,
        PlayerAnimationController animation, IPlayerData player);
    /// <summary>
    /// Stops the player from moving
    /// </summary>
    /// <param name="player"></param>
    /// <param name="animation"></param>
    void StopMoving(IPlayerData player, PlayerAnimationController animation);
}
