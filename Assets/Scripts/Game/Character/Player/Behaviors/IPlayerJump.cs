
/// <summary>
/// Interface for player jump handling
/// </summary>
public interface IPlayerJump
{
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
    /// <summary>
    /// Updates the grounded state
    /// </summary>
    void UpdateGroundState();
}
