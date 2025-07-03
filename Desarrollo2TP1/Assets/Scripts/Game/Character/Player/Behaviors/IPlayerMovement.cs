using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerMovement
{
    float CurrentSpeed { get; set; }
    void Move(Vector3 direction, IPlayerMovementCalculator calculator);
    public void Move(InputAction.CallbackContext context, IPlayerMovementCalculator calculator,
        PlayerAnimationController animation, Player player);
    void StopMoving(Player player, PlayerAnimationController animation);
}
