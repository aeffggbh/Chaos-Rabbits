using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Behavior that allows the player to jump
/// </summary>
public class PlayerJump : IPlayerJump
{
    private Rigidbody _rb;
    private ISoundPlayer _soundPlayer;

    private bool _isJumping;
    private CapsuleCollider _collider;

    /// <summary>
    /// Initializes local variables
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="soundPlayer"></param>
    public PlayerJump(Rigidbody rb, ISoundPlayer soundPlayer, CapsuleCollider collider)
    {
        _rb = rb;
        _soundPlayer = soundPlayer;
        _collider = collider;
    }

    public void SetJumpState(bool isJumping)
    {
        _isJumping = isJumping;
    }

    public void Jump(float force)
    {
        if (IsGrounded())
        {
            _rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            _soundPlayer.PlaySound(SFXType.JUMP);
        }
    }

    public bool IsGrounded()
    {
        float rayLength = 0.1f;
        Vector3 origin = _collider.bounds.center;
        origin.y = _collider.bounds.min.y + 0.01f;

        RaycastHit hit; 
        if (Physics.Raycast(_collider.transform.position, -_collider.transform.up, out hit, rayLength))
        {
            return true;
        }

        return false;
    }

    
}