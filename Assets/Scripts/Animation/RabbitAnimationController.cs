using UnityEngine;

public class RabbitAnimationController : AnimationController
{
    public void TriggerJump()
    {
        CheckAnimator();
        animator.ResetTrigger("jumpRequested");
        animator.SetTrigger("jumpRequested");
    }

    public void UpdateGround(bool isGrounded)
    {
        CheckAnimator();

        if (isGrounded && !animator.GetBool("isGrounded"))
            animator.SetTrigger("hasLanded");

        animator.SetBool("isGrounded", isGrounded);
    }

    public bool IsLanding()
    {
        CheckAnimator();
        bool isLanding = animator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

        return isLanding;
    }
}
