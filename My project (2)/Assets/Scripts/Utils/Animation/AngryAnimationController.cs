using UnityEngine;

/// <summary>
/// Controller for the angry character animations.
/// </summary>
public class AngryAnimationController : AnimationController
{
    public void Attack()
    {
        if (animator != null)
            animator.SetBool("isAttacking", true);
        else
            Debug.LogError("Animator is null for " + gameObject.name);
    }

    public void Die()
    {
        if (animator != null)
            animator.SetBool("isDead", true);
        else
            Debug.LogError("Animator is null for " + gameObject.name);
    }
}
