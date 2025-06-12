using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static SceneController;

/// <summary>
/// Manages the pause menu
/// </summary>
[RequireComponent(typeof(AudioSource))]

public class PauseManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _checkExitMenu;
    [Header("Events")]
    [SerializeField] EventSystem eventSystem;
    [SerializeField] private GameObject _firstButtonA;
    [SerializeField] private GameObject _firstButtonB;
    private bool paused = false;
    private AudioSource _audioSource;
    private SoundManager _soundManager;

    private void Start()
    {
        Time.timeScale = 1f;
        GameManager.SetPause(false);

        if (GameManager.initialized)
            GameManager.pauseButton.action.started += OnPause;
        else
            Debug.LogWarning("GameManager not initialized, cannot set pause button");

        if (!_pauseMenu)
            Debug.LogError(nameof(_pauseMenu) + " is null");

        if (!_checkExitMenu)
            Debug.LogError(nameof(_checkExitMenu) + " is null");

        if (!_audioSource)
            _audioSource = GetComponent<AudioSource>();

        if (ServiceProvider.TryGetService<SoundManager>(out var soundManager))
            _soundManager = soundManager;
    }

    private void Update()
    {
        if (paused && !_pauseMenu.activeSelf)
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
                eventSystem.SetSelectedGameObject(_firstButtonA);
            else
                eventSystem.SetSelectedGameObject(null);

            if (_pauseMenu)
                _pauseMenu.SetActive(paused);

            GameManager.SetPause(paused);

            if (_checkExitMenu)
                _checkExitMenu.SetActive(false);

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
        if (_checkExitMenu)
        {
            _checkExitMenu.SetActive(true);
            if (_pauseMenu)
                _pauseMenu.SetActive(false);
            eventSystem.SetSelectedGameObject(_firstButtonB);
        }

        PlayButtonSound();

    }

    /// <summary>
    /// Returns to the main menu, resetting the game state and time scale.
    /// </summary>
    public void BackToMenu()
    {
        if (_checkExitMenu)
            _checkExitMenu.SetActive(false);
        if (_pauseMenu)
            _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.SetPause(false);
        paused = false;
        SceneController.GoToScene(SceneController.GameState.MAINMENU);

        PlayButtonSound();
    }

    public void BackToPause()
    {
        if (_checkExitMenu)
        {
            _checkExitMenu.SetActive(false);
            _pauseMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(_firstButtonA);
        }

        PlayButtonSound();
    }

    public void ResumeGame()
    {
        if (_pauseMenu)
            _pauseMenu.SetActive(false);
        if (_checkExitMenu)
            _checkExitMenu.SetActive(false);
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
        _soundManager.PlaySound(SoundType.CONFIRM, _audioSource);
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
