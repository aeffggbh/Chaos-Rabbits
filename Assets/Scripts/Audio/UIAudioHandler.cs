using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Can play UI sounds from a single audiosource
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class UIAudioHandler : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioListener _audioListener;
    private ISoundPlayer _soundPlayer;

    private void Awake()
    {
        ServiceProvider.SetService<UIAudioHandler>(this);

        _audioListener = GetComponent<AudioListener>();

        EventProvider.Subscribe<ILevelUpSoundEvent>(PlayLevelUpSound);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<UIAudioHandler>(null);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _soundPlayer = new SoundPlayer(_audioSource);
    }

    /// <summary>
    /// Plays the sound of the button
    /// </summary>
    public void PlayButtonSound()
    {
        _soundPlayer?.PlaySound(SFXType.CONFIRM);
    }

    /// <summary>
    /// Plays a sound for levelling up
    /// </summary>
    public void PlayLevelUpSound(ILevelUpSoundEvent newLevelEvent)
    {
        _soundPlayer?.PlaySound(SFXType.LEVEL_UP);
    }
}
