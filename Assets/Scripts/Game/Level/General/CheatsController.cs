using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller for cheats in the game.
/// </summary>
/// 
[CreateAssetMenu(fileName = "CheatsController", menuName = "ScriptableObjects/CheatsController")]
public class CheatsController : ScriptableObject
{
    [SerializeField] private InputActionReference _nextLevel;
    [SerializeField] private InputActionReference _godMode;
    [SerializeField] private InputActionReference _flashMode;
    private bool _isGodMode;
    private bool _isFlashMode;

    public bool IsGodMode()
    {
        return _isGodMode;
    }

    public bool IsFlashMode()
    {
        return _isFlashMode;
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

    }

    /// <summary>
    /// Toggles the flash mode on or off.
    /// </summary>
    /// <param name="context"></param>
    private void OnFlashMode(InputAction.CallbackContext context)
    {
        ServiceProvider.TryGetService<GameSceneController>(out var controller);
        if (controller.IsTypeLoaded<GameplaySceneData>() && !PauseManager.Paused)
        {
            _isFlashMode = !_isFlashMode;
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(null, "Flash Mode", _isFlashMode));
        }
    }

    /// <summary>
    /// Toggles the god mode on or off.
    /// </summary>
    /// <param name="context"></param>
    private void OnGodMode(InputAction.CallbackContext context)
    {
        ServiceProvider.TryGetService<GameSceneController>(out var controller);
        if (controller.IsTypeLoaded<GameplaySceneData>() && !PauseManager.Paused)
        {
            _isGodMode = !_isGodMode;
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(null, "God Mode", _isGodMode));
        }
    }

    /// <summary>
    /// Advances to the next level in the game.
    /// </summary>
    /// <param name="context"></param>
    private void OnNextLevel(InputAction.CallbackContext context)
    {
        GoToNextLevel();
    }

    private void GoToNextLevel()
    {
        ServiceProvider.TryGetService<GameSceneController>(out var controller);
        if (controller.IsTypeLoaded<GameplaySceneData>() && !PauseManager.Paused)
        {
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(null, true));
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(null, "Gone to next level"));
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
