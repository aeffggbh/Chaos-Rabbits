using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIAudioHandler : MonoBehaviour
{
    private AudioSource _audioSource;
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
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _soundPlayer = new SoundPlayer(_audioSource);
    }

    public void PlaySound()
    {
        if (_soundPlayer != null) 
            _soundPlayer.PlaySound(SFXType.CONFIRM);
    }
}
