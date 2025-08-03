using UnityEngine;

/// <summary>
/// Controlls player animations
/// </summary>
public class PlayerAnimationController : JumpAnimationController
{
    public void AnimateShoot()
    {
        CheckAnimator();
        animator?.SetTrigger("isShooting");
    }

    /// <summary>
    /// Plays the walk animation
    /// </summary>
    public void AnimateWalk()
    {
        CheckAnimator();
        animator?.SetBool("isWalking", true);
    }

    /// <summary>
    /// Transitions from walk to idle animation
    /// </summary>
    public void AnimateStopWalking()
    {
        CheckAnimator();
        animator?.SetBool("isWalking", false);
    }

    /// <summary>
    /// Switches from the idle animation to the grab animation
    /// </summary>
    public void AnimateGrabWeapon()
    {
        CheckAnimator();
        animator?.SetBool("isAiming", true);
    }

    /// <summary>
    /// Switches the grab animation to the idle animation
    /// </summary>
    public void AnimateDropWeapon()
    {
        CheckAnimator();
        animator?.SetBool("isAiming", false);
    }
}
