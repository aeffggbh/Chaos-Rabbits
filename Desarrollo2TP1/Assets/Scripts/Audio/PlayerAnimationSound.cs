public class PlayerAnimationSound : AnimationSound
{
    protected void PlayFootstepSound()
    {
        PlayAnimationSound(SFXType.FOOTSTEPS, 0.7f);
    }
}