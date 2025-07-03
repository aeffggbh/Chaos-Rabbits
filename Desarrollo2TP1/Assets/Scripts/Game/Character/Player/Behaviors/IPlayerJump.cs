using UnityEngine;

public interface IPlayerJump
{
    void SetJumpState(bool isJumping);
    void Jump(float force);
    bool IsGrounded { get; set; }
}
