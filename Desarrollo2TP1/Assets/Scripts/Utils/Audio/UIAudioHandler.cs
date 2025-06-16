using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIAudioHandler : MonoBehaviour
{
    private AudioSource _audioSource;
    private SoundManager _soundManager;
    public static UIAudioHandler instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject, 1f);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;
    }

    public void PlaySound()
    {
        //in back to menu it still doesnt work very well
        _soundManager?.PlaySound(SFXType.CONFIRM, _audioSource);
    }
}
