
/// <summary>
/// Controls the weapon animations
/// </summary>
public class WeaponAnimationController : AnimationController
{
    /// <summary>
    /// Plays the shoot animation
    /// </summary>
    public void AnimateShoot()
    {
        CheckAnimator();

        if (animator != null && animator.enabled)
            animator.Play("Shoot");
    }
}
