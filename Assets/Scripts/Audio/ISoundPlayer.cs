using UnityEngine;

/// <summary>
/// Interface to play sounds 
/// </summary>
public interface ISoundPlayer
{
    void PlaySound(string key, float volume = 1f);

    void SetAudioSource(AudioSource audioSource);
}
