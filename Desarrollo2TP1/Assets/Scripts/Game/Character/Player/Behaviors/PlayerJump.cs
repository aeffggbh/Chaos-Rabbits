using UnityEngine;

public class PlayerJump : IPlayerJump
{
    private Rigidbody _rb;
    private readonly ISoundPlayer _soundPlayer;

    public bool IsGrounded { get; set; }
    private bool jumpTrigger;

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
                _rb.AddForce(Vector3.up * force /** Time.fixedDeltaTime*/, ForceMode.Impulse);
                IsGrounded = false;
                _soundPlayer.PlaySound(SFXType.JUMP);
            }

            jumpTrigger = false;
        }
    }
}