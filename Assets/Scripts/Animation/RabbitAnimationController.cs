using UnityEngine;

public class RabbitAnimationController : AnimationController
{
    public void TriggerJump()
    {
        CheckAnimator();
        animator.ResetTrigger("jumpRequested");
        animator.SetTrigger("jumpRequested");
        animator.SetBool("hasLanded", false);
    }

    public void UpdateGround(bool isGrounded)
    {
        CheckAnimator();

        if (isGrounded && !animator.GetBool("isGrounded"))
        {
            animator.SetBool("hasLanded", true);
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    public bool IsLanding()
    {
        CheckAnimator();
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }
}
