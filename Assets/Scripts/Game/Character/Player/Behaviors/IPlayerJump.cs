
/// <summary>
/// Interface for player jump handling
/// </summary>
public interface IPlayerJump
{
    /// <summary>
    /// Sets a jump state with the provided bool
    /// </summary>
    /// <param name="isJumping"></param>
    void SetJumpState(bool isJumping);
    /// <summary>
    /// Applies the force provided and jumps with it
    /// </summary>
    /// <param name="force"></param>
    void Jump(float force);
    /// <summary>
    /// Checks if the player is grounded by casting a ray downwards from the feet position
    /// </summary>
    /// <returns></returns>
    bool IsGrounded();
}
