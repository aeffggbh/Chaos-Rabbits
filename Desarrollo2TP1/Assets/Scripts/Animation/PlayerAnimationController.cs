using UnityEngine;

/// <summary>
/// Controlls player animations
/// </summary>
public class PlayerAnimationController : AnimationController
{
    /// <summary>
    /// Plays the walk animation
    /// </summary>
    public void AnimateWalk()
    {
        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    /// <summary>
    /// Transitions from walk to idle animation
    /// </summary>
    public void AnimateStopWalking()
    {
        if (animator != null)
            animator.SetBool("isWalking", false);
    }

    /// <summary>
    /// Switches from the idle animation to the grab animation
    /// </summary>
    public void AnimateGrabWeapon()
    {
        if (animator != null)
            animator.SetBool("isAiming", true);
    }

    /// <summary>
    /// Switches the grab animation to the idle animation
    /// </summary>
    public void AnimateDropWeapon()
    {
        if (animator != null)
            animator.SetBool("isAiming", false);
    }
}
