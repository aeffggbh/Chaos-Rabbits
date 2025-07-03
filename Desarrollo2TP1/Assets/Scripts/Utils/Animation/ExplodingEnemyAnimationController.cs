using UnityEngine;

/// <summary>
/// Controller for the angry character animations.
/// </summary>
public class ExplodingEnemyAnimationController : AnimationController
{
    public void Attack()
    {
        if (animator != null)
            animator.SetBool("isAttacking", true);
        else
            Debug.LogError("Animator is null for " + gameObject.name);
    }

    //TODO: use this.
    public void Death()
    {
        if (animator != null)
            animator.SetBool("isDead", true);
        else
            Debug.LogError("Animator is null for " + gameObject.name);
    }
}
