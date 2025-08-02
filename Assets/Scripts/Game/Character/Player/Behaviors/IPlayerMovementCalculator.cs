
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Interface for handling all the calculations that the movement needs
/// </summary>
public interface IPlayerMovementCalculator
{
    /// <summary>
    /// A camera that is used for movement calculations
    /// </summary>
    CinemachineCamera Camera { get; }
    /// <summary>
    /// Gets the movement direction according to where the camera is pointing
    /// </summary>
    /// <param name="moveInput"></param>
    /// <returns></returns>
    Vector3 GetDirection(Vector2 moveInput);
}