using UnityEngine;
using UnityEngine.UI;
using static SceneController;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class SceneLoaderButtonHandler : MonoBehaviour
{
    [SerializeField] GameState state;
    private SoundManager _soundManager;
    private Button _button;
    private AudioSource _audioSource;
    private void Reset()
    {
        _button = GetComponent<Button>();
    }

    private void Awake()
    {
        _button ??= GetComponent<Button>();
    }

    private void Start()
    {
        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;
        _audioSource ??= GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        if (_soundManager == null)
            if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
                _soundManager = soundManager;

        if (_soundManager != null)
            _soundManager.PlaySound(SoundType.CONFIRM, _audioSource);
        else
            Debug.LogError(nameof(_soundManager) + " doesn't exist");

        SceneController.GoToScene(state);
    }

}
