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

    private void CheckListener(IActivateSceneEvent sceneEvent)
    {
        if (sceneEvent is ActivateGameplayEvent)
            if (_audioListener != null)
                _audioListener.enabled = false;
    }

    public void ActivateAudioListener()
    {
        if (_audioListener != null)
            _audioListener.enabled = true;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _soundPlayer = new SoundPlayer(_audioSource);
    }

    public void PlayButtonSound()
    {
        if (_soundPlayer != null)
            _soundPlayer.PlaySound(SFXType.CONFIRM);
    }
}
