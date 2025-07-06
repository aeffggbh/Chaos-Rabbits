using UnityEngine;

public class PlayerAnimationController : AnimationController
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

    public void GrabWeapon()
    {
        if (animator != null)
            animator.SetBool("isAiming", true);
    }

    public void DropWeapon()
    {
        if (animator != null)
            animator.SetBool("isAiming", false);
    }
}
