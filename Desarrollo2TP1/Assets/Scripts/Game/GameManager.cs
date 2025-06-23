using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages overal gameplay variables and replayability
/// </summary>
public static class GameManager
{
    public static bool initialized = false;
    public static PlayerController savedPlayer;
    public static Weapon defaultWeapon;
    public static InputActionReference pauseButton;
    public static bool paused = false;
    public static SoundManager soundManager;

    /// <summary>
    /// Checks if the game should be reset based on the target scene.
    /// </summary>
    /// <param name="targetScene"></param>
    /// <returns></returns>
    public static bool ShouldReset(SceneController.GameState targetScene)
    {
        return targetScene == SceneController.GameState.MAINMENU ||
            SceneController.previousScene == SceneController.GameState.GAMEOVER ||
            SceneController.previousScene == SceneController.GameState.GAMEWIN;
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    public static void ResetGame()
    {
        if (initialized)
        {
            if (CheatsController.instance)
                CheatsController.instance.Reset();

            if (savedPlayer != null)
            {
                GameObject.Destroy(savedPlayer.gameObject);
                savedPlayer = null;
            }

            CinemachineBrain _cineMachineBrain = null;

            if (ServiceProvider.TryGetService<CinemachineBrain>(out var cinemachineBrain))
                _cineMachineBrain = cinemachineBrain;

            if (_cineMachineBrain != null)
                GameObject.Destroy(_cineMachineBrain.gameObject);

            initialized = false;
        }
    }

    /// <summary>
    /// Sets the pause state of the game.
    /// </summary>
    /// <param name="paused"></param>
    public static void SetPause(bool paused)
    {
        GameManager.paused = paused;
    }
}
