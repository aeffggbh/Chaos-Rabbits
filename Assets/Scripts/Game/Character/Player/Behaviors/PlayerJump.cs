using UnityEngine;

/// <summary>
/// Behavior that allows the player to jump
/// </summary>
public class PlayerJump : IPlayerJump
{
    private Rigidbody _rb;
    private ISoundPlayer _soundPlayer;

    public bool IsGrounded { get; set; }
    private bool jumpTrigger;

    /// <summary>
    /// Initializes local variables
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="soundPlayer"></param>
    public PlayerJump(Rigidbody rb, ISoundPlayer soundPlayer)
    {
        _rb = rb;
        _soundPlayer = soundPlayer;
    }

    public void SetJumpState(bool isJumping)
    {
        jumpTrigger = isJumping;
    }

    public void Jump(float force)
    {
        if (jumpTrigger)
        {
            if (IsGrounded)
            {
                _rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                IsGrounded = false;
                _soundPlayer.PlaySound(SFXType.JUMP);
            }

            jumpTrigger = false;
        }
    }
}