
/// <summary>
/// Controller for clown animations.
/// </summary>
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

    //todo: USE THIS
    public void Death()
    {
        if (animator != null)
            animator.SetBool("isDead", true);
    }
}
