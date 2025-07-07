using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimationSound : MonoBehaviour
{
    private ISoundPlayer _soundPlayer;

    private void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        _soundPlayer = new SoundPlayer(audioSource);
    }

    protected void PlayAnimationSound(SFXType type, float volume = 1f)
    {
        _soundPlayer.PlaySound(type, volume);
    }
}
