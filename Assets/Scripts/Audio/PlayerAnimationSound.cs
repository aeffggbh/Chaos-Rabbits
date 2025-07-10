/// <summary>
/// Plays a sound for a player animation
/// </summary>
public class PlayerAnimationSound : AnimationSound
{
    protected void PlayFootstepSound()
    {
        PlayAnimationSound(SFXType.FOOTSTEPS, 0.5f);
    }
}