
public class ClownAnimationController : AnimationController
{
    public void Walk()
    {
        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    public void StopWalking()
    {
        if (animator != null)
            animator.SetBool("isWalking", false);
    }

    public void Death()
    {
        if (animator != null)
            animator.SetBool("isDead", true);
    }
}
