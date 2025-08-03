using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Behavior that allows the player to jump
/// </summary>
public class PlayerJump : IPlayerJump
{
    private Rigidbody _rb;
    private ISoundPlayer _soundPlayer;

    private CapsuleCollider _collider;
    private PlayerAnimationController _playerAnimationController;
    private bool _grounded;

    /// <summary>
    /// Initializes local variables
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="soundPlayer"></param>
    public PlayerJump(Rigidbody rb, ISoundPlayer soundPlayer, CapsuleCollider collider, PlayerAnimationController animationController)
    {
        _rb = rb;
        _soundPlayer = soundPlayer;
        _collider = collider;
        _playerAnimationController = animationController;
    }

    public void Jump(float force)
    {
        if (_grounded)
        {
            _rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            _soundPlayer.PlaySound("jump");
            _playerAnimationController.AnimateTriggerJump();
        }
    }

    public bool IsGrounded()
    {
        bool grounded = RayManager.IsGrounded(_collider);

        _playerAnimationController.AnimateGrounded(grounded);

        return grounded;
    }

    public void UpdateGroundState()
    {
        _grounded = IsGrounded();
    }
}