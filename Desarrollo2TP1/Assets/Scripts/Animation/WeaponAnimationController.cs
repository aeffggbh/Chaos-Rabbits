
public class WeaponAnimationController : AnimationController
{
    public void Shoot()
    {
        CheckAnimator();

        if (animator != null && animator.enabled)
            animator.Play("Shoot");
    }
}
