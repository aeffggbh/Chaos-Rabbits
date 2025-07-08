using UnityEngine;

/// <summary>
/// Interface to play sounds 
/// </summary>
public interface ISoundPlayer
{
    void PlaySound(SFXType type, float volume = 1f);

    void SetAudioSource(AudioSource audioSource);
}
