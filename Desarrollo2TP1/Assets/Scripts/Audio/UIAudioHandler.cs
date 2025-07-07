using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class UIAudioHandler : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioListener _audioListener;
    public static UIAudioHandler Instance;
    private ISoundPlayer _soundPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _audioListener = GetComponent<AudioListener>();

        EventProvider.Subscribe<IActivateSceneEvent>(CheckListener);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _soundPlayer = new SoundPlayer(_audioSource);
    }

    /// <summary>
    /// Checks if the listener should be disabled. It happens when I activate the gameplay
    /// </summary>
    /// <param name="sceneEvent"></param>
    private void CheckListener(IActivateSceneEvent sceneEvent)
    {
        if (sceneEvent is ActivateGameplayEvent)
            if (_audioListener != null)
                _audioListener.enabled = false;
    }
    
    /// <summary>
    /// Enables the UI audio listener.
    /// </summary>
    public void ActivateAudioListener()
    {
        if (_audioListener != null)
            _audioListener.enabled = true;
    }

    /// <summary>
    /// Plays the sound of the button
    /// </summary>
    public void PlayButtonSound()
    {
        if (_soundPlayer != null)
            _soundPlayer.PlaySound(SFXType.CONFIRM);
    }
}
