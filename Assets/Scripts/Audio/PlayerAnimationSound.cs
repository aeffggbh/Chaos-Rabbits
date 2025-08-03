using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Plays a sound for a player animation
/// </summary>
public class PlayerAnimationSound : AnimationSoundPlayer
{
    private CapsuleCollider _collider;
    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<CapsuleCollider>();
    }

    protected void PlayFootstepSound()
    {
        if (RayManager.IsGrounded(_collider))
            PlayAnimationSound("footsteps", 0.5f);
    }
}