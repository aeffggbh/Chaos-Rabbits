using UnityEngine;

public interface ISoundPlayer
{
    void PlaySound(SFXType type);

    void SetAudioSource(AudioSource audioSource);
}
