
using UnityEngine;

/// <summary>
/// Interface for handling all the calculations that the movement needs
/// </summary>
public interface IPlayerMovementCalculator
{
    /// <summary>
    /// A camera that is used for movement calculations
    /// </summary>
    Camera Camera { get; }
    /// <summary>
    /// Gets the movement direction according to where the camera is pointing
    /// </summary>
    /// <param name="moveInput"></param>
    /// <returns></returns>
    Vector3 GetDirection(Vector2 moveInput);
    /// <summary>
    /// Sets a camera for future calculations
    /// </summary>
    /// <param name="camera"></param>
    void SetCamera(Camera camera);
}