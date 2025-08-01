
/// <summary>
/// Controller for clown animations.
/// </summary>
public class ShooterAnimationController : AnimationController
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
    /// Transitions from walk animation to idle
    /// </summary>
    public void AnimateStopWalking()
    {
        if (animator != null)
            animator.SetBool("isWalking", false);
    }

    public void AnimateTriggerJump()
    {
        CheckAnimator();
        animator.ResetTrigger("jumpRequested");
        animator.SetTrigger("jumpRequested");
    }

    public void AnimateGrounded(bool isGrounded)
    {
        CheckAnimator();

        if (isGrounded && !animator.GetBool("isGrounded"))
            animator.SetTrigger("hasLanded");

        animator.SetBool("isGrounded", isGrounded);
    }

    public bool IsLandingAnimationPlaying()
    {
        CheckAnimator();
        bool isLanding = animator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

        return isLanding;
    }
}
