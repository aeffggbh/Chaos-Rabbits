using UnityEngine;

/// <summary>
/// Can play sounds given an audiosource
/// </summary>
[RequireComponent(typeof(AudioSource))]
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

    public void PlaySound(SFXType type, float volume = 1f)
    {
        if (PauseManager.Paused && type != SFXType.CONFIRM) 
            return;
        _soundManager.PlaySound(type, _audioSource, volume);
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }
}