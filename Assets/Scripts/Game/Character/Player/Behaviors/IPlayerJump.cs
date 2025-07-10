
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
    /// Saves the grounded state of the player
    /// </summary>
    bool IsGrounded { get; set; }
}
