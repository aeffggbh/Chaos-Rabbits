using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementCalculator : IPlayerMovementCalculator
{
    private CinemachineBrain _cineMachineBrain;
    public Camera Camera { get; set; }

    public PlayerMovementCalculator()
    {
        _cineMachineBrain = CineMachineManager.Instance?.cineMachineBrain;
        Camera = _cineMachineBrain?.GetComponent<Camera>();
    }

    public void SetCamera(Camera camera)
    {
        Camera = camera;
    }

    public Vector3 GetDirection(Vector2 moveInput)
    {
        if (!Camera)
            return new(moveInput.x, 0, moveInput.y);

        Vector3 forward = Camera.transform.forward;
        Vector3 right = Camera.transform.right;

        forward.y = 0;
        right.y = 0;

        return (forward * moveInput.y + right * moveInput.x).normalized;
    }
}