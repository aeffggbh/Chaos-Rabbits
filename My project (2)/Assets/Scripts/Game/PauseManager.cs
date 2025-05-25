using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static SceneController;

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

    private void Start()
    {
        if (GameManager.initialized)
            GameManager.pauseButton.action.started += OnPause;
        else
            Debug.LogWarning("GameManager not initialized, cannot set pause button");

        if (!_pauseMenu)
            Debug.LogError(nameof(_pauseMenu) + " is null");

        if (!_checkExitMenu)
            Debug.LogError(nameof(_checkExitMenu) + " is null");

    }

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

            CheckCursor();
        }
        else
            Debug.LogWarning("GameManager not initialized, cannot pause");
    }

    public void CheckExit()
    {
        if (_checkExitMenu)
        {
            _checkExitMenu.SetActive(true);
            if (_pauseMenu)
                _pauseMenu.SetActive(false);
            eventSystem.SetSelectedGameObject(_firstButtonB);
        }
    }

    public void BackToMenu()
    {
        if (_checkExitMenu)
            _checkExitMenu.SetActive(false);
        if (_pauseMenu)
            _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.SetPause(false);
        paused = false;
        SceneController.GoToScene(SceneController.Scenes.MAINMENU);
    }

    public void BackToPause()
    {
        if (_checkExitMenu)
        {
            _checkExitMenu.SetActive(false);
            _pauseMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(_firstButtonA);
        }
    }

    public void ResumeGame()
    {
        if (_pauseMenu)
            _pauseMenu.SetActive(false);
        if (_checkExitMenu)
            _checkExitMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.SetPause(false);
    }

    public void ExitGame()
    {
        SceneController.ExitGame();
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
