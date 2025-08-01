
public class JumpAnimationController : AnimationController
{
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