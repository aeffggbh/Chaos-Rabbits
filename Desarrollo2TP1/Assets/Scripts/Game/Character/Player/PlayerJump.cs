using UnityEngine;

public class PlayerJump : IPlayerJump
{
    private readonly Rigidbody _rb;
    private readonly ISoundPlayer _soundPlayer;

    public bool IsGrounded { get; set; }

    public PlayerJump(Rigidbody rb, ISoundPlayer soundPlayer)
    {
        _rb = rb;
        _soundPlayer = soundPlayer;
    }

    public void Jump(float force, bool shouldJump)
    {
        if (shouldJump)
        {
            if (IsGrounded)
            {
                _rb.AddForce(Vector3.up * force * Time.fixedDeltaTime, ForceMode.Impulse);
                IsGrounded = false;
                _soundPlayer.PlaySound(SFXType.JUMP);
            }

            shouldJump = false;
        }
    }
}