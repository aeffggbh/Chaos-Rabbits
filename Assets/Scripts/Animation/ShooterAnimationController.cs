
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
}
