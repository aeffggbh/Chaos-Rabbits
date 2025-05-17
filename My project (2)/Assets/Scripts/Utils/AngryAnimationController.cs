using UnityEngine;

public class AngryAnimationController : AnimationController
{
    public void Attack()
    {
        if (animator != null)
            animator.SetBool("isAttacking", true);
        else
            Debug.LogError("Animator is null for " + gameObject.name);
    }
}
