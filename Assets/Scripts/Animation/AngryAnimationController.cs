using UnityEngine;

/// <summary>
/// Controller for the angry character animations.
/// </summary>
public class AngryAnimationController : AnimationController
{
    /// <summary>
    /// Plays the attack animation
    /// </summary>
    public void AnimateAttack()
    {
        CheckAnimator();

        animator.SetBool("isAttacking", true);
    }
}
