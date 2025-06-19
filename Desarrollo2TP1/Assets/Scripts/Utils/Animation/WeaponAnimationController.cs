using UnityEngine;

public class WeaponAnimationController : AnimationController
{
    public void Shoot()
    {
        CheckAnimator();

        if (animator != null && animator.enabled)
            animator.Play("Shoot");
    }

    public void ActivateAnimation()
    {
        CheckAnimator();

        animator.enabled = true;
    }


    public void DeactivateAnimation()
    {
        CheckAnimator();

        animator.enabled = false;
    }
}
