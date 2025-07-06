using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller for cheats in the game.
/// </summary>
public class CheatsController : MonoBehaviour
{
    [SerializeField] private InputActionReference _nextLevel;
    [SerializeField] private InputActionReference _godMode;
    [SerializeField] private InputActionReference _flashMode;
    [SerializeField] public Transform levelTriggerLocation;
    [SerializeField] public CheatsLogger logger;
    private bool _isGodMode;
    private bool _isFlashMode;

    public static CheatsController Instance;

    public bool IsGodMode()
    {
        return _isGodMode;
    }

    public bool IsFlashMode()
    {
        return _isFlashMode;
    }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        if (!_nextLevel)
            Debug.LogError(nameof(_nextLevel) + " is null");
        else
            _nextLevel.action.performed += OnNextLevel;

        if (!_godMode)
            Debug.LogError(nameof(_godMode) + " is null");
        else
            _godMode.action.performed += OnGodMode;

        if (!_flashMode)
            Debug.LogError(nameof(_flashMode) + " is null");
        else
            _flashMode.action.performed += OnFlashMode;

        if (!logger)
            GetComponent<CheatsLogger>();
    }

    /// <summary>
    /// Toggles the flash mode on or off.
    /// </summary>
    /// <param name="context"></param>
    private void OnFlashMode(InputAction.CallbackContext context)
    {
        if (GameSceneController.Instance.IsTypeLoaded<GameplayScene>() && !PauseManager.Paused)
        {
            _isFlashMode = !_isFlashMode;
            logger.RequestText("Flash Mode", _isFlashMode);
        }
    }

    /// <summary>
    /// Toggles the god mode on or off.
    /// </summary>
    /// <param name="context"></param>
    private void OnGodMode(InputAction.CallbackContext context)
    {
        if (GameSceneController.Instance.IsTypeLoaded<GameplayScene>() && !PauseManager.Paused)
        {
            _isGodMode = !_isGodMode;
            logger.RequestText("God Mode", _isGodMode);
        }
    }

    /// <summary>
    /// Advances to the next level in the game.
    /// </summary>
    /// <param name="context"></param>
    private void OnNextLevel(InputAction.CallbackContext context)
    {
        if (Instance)
            GoToNextLevel();
    }

    private void GoToNextLevel()
    {
        if (GameSceneController.Instance.IsTypeLoaded<GameplayScene>() && !PauseManager.Paused)
        {
            //if (ServiceProvider.TryGetService<PlayerMediator>(out var playerController))
            //    playerController.transform.position = levelTriggerLocation.position;

            PlayerMediator.PlayerInstance.transform.position = levelTriggerLocation.position;

            //SceneLoader.Instance.LoadNextLevel();
            //TODO: make a bool to know if it should actually load the new level and unload the previous
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, true));

            logger.RequestText("Gone to next level");
        }
    }

    /// <summary>
    /// Resets the cheats controller to its initial state.
    /// </summary>
    public void Reset()
    {
        _isGodMode = false;
        _isFlashMode = false;
    }
}
