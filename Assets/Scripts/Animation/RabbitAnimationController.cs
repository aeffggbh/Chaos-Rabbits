using UnityEngine;

public class RabbitAnimationController : AnimationController
{
    public void TriggerJump()
    {
        CheckAnimator();
        animator.ResetTrigger("jumpRequested");
        animator.SetTrigger("jumpRequested");
        Debug.Log("JUMP");
    }

    public void UpdateGround(bool isGrounded)
    {
        CheckAnimator();

        if (isGrounded && !animator.GetBool("isGrounded"))
        {
            animator.SetTrigger("hasLanded");
            Debug.Log("LAND");
        }
        else if (isGrounded)
            Debug.Log("GROUNDED");

        animator.SetBool("isGrounded", isGrounded);
    }

    public bool IsLanding()
    {
        CheckAnimator();
        bool isLanding = animator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

        if (isLanding)
            Debug.Log("ACTUALLY LAND");

        return isLanding;
    }
}
