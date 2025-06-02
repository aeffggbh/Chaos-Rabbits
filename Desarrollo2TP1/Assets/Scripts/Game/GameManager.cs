using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages overal gameplay variables
/// </summary>
public static class GameManager
{
    //responsible for replayability and general game variables
    public static CheatsController cheatsController;
    public static bool initialized = false;
    public static PlayerController savedPlayer;
    public static Weapon defaultWeapon;
    public static InputActionReference pauseButton;
    public static bool paused = false;

    /// <summary>
    /// Checks if the game should be reset based on the target scene.
    /// </summary>
    /// <param name="targetScene"></param>
    /// <returns></returns>
    public static bool ShouldReset(SceneController.GameStates targetScene)
    {
        return targetScene == SceneController.GameStates.MAINMENU ||
            SceneController.previousScene == SceneController.GameStates.GAMEOVER ||
            SceneController.previousScene == SceneController.GameStates.GAMEWIN;
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    public static void ResetGame()
    {
        if (initialized)
        {
            cheatsController.Reset();
            //savedPlayer.currentWeapon = defaultWeapon;
            //savedPlayer.ResetPlayer();
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
