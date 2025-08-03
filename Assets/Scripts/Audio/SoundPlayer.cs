using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

/// <summary>
/// Can play sounds given an audiosource
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : ISoundPlayer
{
    private AudioSource _audioSource;
    private SoundContainer _soundManager;

    public SoundPlayer(AudioSource audioSource)
    {
        _audioSource = audioSource;
        if (ServiceProvider.TryGetService<SoundContainer>(out var soundManager))
            _soundManager = soundManager;
    }

    public void PlaySound(string key, float volume = 1f)
    {
        if (PauseManager.Paused && (key != "confirm" && key != "navigation")) 
            return;

        var clip = _soundManager?.GetClip(key);
        _audioSource?.PlayOneShot(clip, volume);
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }
}