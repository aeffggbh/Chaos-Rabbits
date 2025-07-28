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
        animator?.SetBool("isWalking", true);
    }

    /// <summary>
    /// Transitions from walk to idle animation
    /// </summary>
    public void AnimateStopWalking()
    {
        animator?.SetBool("isWalking", false);
    }

    /// <summary>
    /// Switches from the idle animation to the grab animation
    /// </summary>
    public void AnimateGrabWeapon()
    {
        animator?.SetBool("isAiming", true);
    }

    /// <summary>
    /// Switches the grab animation to the idle animation
    /// </summary>
    public void AnimateDropWeapon()
    {
        animator?.SetBool("isAiming", false);
    }

    public void AnimateGrenadeThrow()
    {
        animator?.SetTrigger("throwGrenade");
    }
}
