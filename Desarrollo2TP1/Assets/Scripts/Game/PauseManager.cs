using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the pause menu
/// </summary>
[RequireComponent(typeof(AudioSource))]

public class PauseManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject _pauseMenuGO;
    [SerializeField] private GameObject _checkExitMenuGO;
    [Header("Events")]
    [SerializeField] EventSystem eventSystem;
    [SerializeField] private GameObject _firstButtonAGO;
    [SerializeField] private GameObject _firstButtonBGO;
    private bool paused = false;
    private AudioSource _audioSource;
    private ISoundPlayer _soundPlayer;

    private void Start()
    {
        Time.timeScale = 1f;
        GameManager.SetPause(false);

        if (GameManager.initialized)
            GameManager.pauseButton.action.started += OnPause;
        else
            Debug.LogWarning("GameManager not initialized, cannot set pause button");

        if (!_pauseMenuGO)
            Debug.LogError(nameof(_pauseMenuGO) + " is null");

        if (!_checkExitMenuGO)
            Debug.LogError(nameof(_checkExitMenuGO) + " is null");

        if (!_audioSource)
            _audioSource = GetComponent<AudioSource>();

        _soundPlayer = new SoundPlayer(_audioSource);
    }

    private void Update()
    {
        if (paused && !_pauseMenuGO.activeSelf)
            paused = false;
    }

    /// <summary>
    /// Handles the pause action when the pause button is pressed.
    /// </summary>
    /// <param name="context"></param>
    private void OnPause(InputAction.CallbackContext context)
    {
        if (GameManager.initialized)
        {
            paused = !paused;

            Time.timeScale = paused ? 0f : 1f;

            if (paused)
                eventSystem.SetSelectedGameObject(_firstButtonAGO);
            else
                eventSystem.SetSelectedGameObject(null);

            if (_pauseMenuGO)
                _pauseMenuGO.SetActive(paused);

            GameManager.SetPause(paused);

            if (_checkExitMenuGO)
                _checkExitMenuGO.SetActive(false);

        }
        else
            Debug.LogWarning("GameManager not initialized, cannot pause");

        CheckCursor();
    }

    /// <summary>
    /// Checks if the exit menu should be displayed and sets it active if so.
    /// </summary>
    public void CheckExit()
    {
        if (_checkExitMenuGO)
        {
            _checkExitMenuGO.SetActive(true);
            if (_pauseMenuGO)
                _pauseMenuGO.SetActive(false);
            eventSystem.SetSelectedGameObject(_firstButtonBGO);
        }

        PlayButtonSound();

    }

    /// <summary>
    /// Returns to the main menu, resetting the game state and time scale.
    /// </summary>
    public void BackToMenu()
    {
        if (_checkExitMenuGO)
            _checkExitMenuGO.SetActive(false);
        if (_pauseMenuGO)
            _pauseMenuGO.SetActive(false);
        GameManager.SetPause(false);
        paused = false;

        Time.timeScale = 1f;

        PlayButtonSound();
        SceneController.GoToScene(SceneController.GameState.MAINMENU);
    }

    public void BackToPause()
    {
        if (_checkExitMenuGO)
        {
            _checkExitMenuGO.SetActive(false);
            _pauseMenuGO.SetActive(true);
            eventSystem.SetSelectedGameObject(_firstButtonAGO);
        }

        PlayButtonSound();
    }

    public void ResumeGame()
    {
        if (_pauseMenuGO)
            _pauseMenuGO.SetActive(false);
        if (_checkExitMenuGO)
            _checkExitMenuGO.SetActive(false);
        Time.timeScale = 1f;
        GameManager.SetPause(false);
        paused = false;
        PlayButtonSound();
        CheckCursor();
    }

    public void ExitGame()
    {
        SceneController.ExitGame();
    }

    private void PlayButtonSound()
    {
        _soundPlayer.PlaySound(SFXType.CONFIRM);
    }

    private void CheckCursor()
    {
        if (!paused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
