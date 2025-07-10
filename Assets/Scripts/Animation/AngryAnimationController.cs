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
        if (animator != null)
            animator.SetBool("isAttacking", true);
        else
            Debug.LogError("Animator is null for " + gameObject.name);
    }
}
