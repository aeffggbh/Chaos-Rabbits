using UnityEngine;

/// <summary>
/// Plays a sound in an animation
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AnimationSoundPlayer : MonoBehaviour
{
    private ISoundPlayer _soundPlayer;

    virtual protected void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        _soundPlayer = new SoundPlayer(audioSource);
    }

    /// <summary>
    /// Plays a sound
    /// </summary>
    /// <param name="type"></param>
    /// <param name="volume"></param>
    protected void PlayAnimationSound(SFXType type, float volume = 1f)
    {
        _soundPlayer.PlaySound(type, volume);
    }
}
