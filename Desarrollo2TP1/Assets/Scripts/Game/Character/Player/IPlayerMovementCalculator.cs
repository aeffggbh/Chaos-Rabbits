
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerMovementCalculator
{
    Camera Camera { get; set; }

    Vector3 GetDirection(Vector2 moveInput);
    void SetCamera(Camera camera);
}