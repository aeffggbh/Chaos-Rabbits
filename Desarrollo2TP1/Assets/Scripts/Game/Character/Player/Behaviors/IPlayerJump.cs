using UnityEngine;

public interface IPlayerJump
{
    void Jump(float force, bool shouldJump);
    bool IsGrounded { get; set; }
}
