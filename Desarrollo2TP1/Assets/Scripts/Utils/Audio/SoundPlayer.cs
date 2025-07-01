

using UnityEngine;
using UnityEngine.Playables;

public class SoundPlayer : ISoundPlayer
{
    private AudioSource _audioSource;
    private SoundManager _soundManager;

    public SoundPlayer(AudioSource audioSource)
    {
        _audioSource = audioSource;
        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;
    }

    public void PlaySound(SFXType type)
    {
        _soundManager.PlaySound(type, _audioSource);
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }
}