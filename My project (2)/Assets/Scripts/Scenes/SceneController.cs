using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for managing scene transitions and game states.
/// </summary>
public static class SceneController
{
    public enum GameStates
    {
        MAINMENU,
        LEVEL1,
        LEVEL2,
        FINAL_LEVEL,
        GAMEWIN,
        CHECKEXIT,
        GAMEOVER,
        CREDITS,
        NONE
    }

    public static GameStates currentScene = GameStates.NONE;
    //so I can go back to the previous scene if I put "no" on checkexit
    public static GameStates previousScene = GameStates.NONE;
    public static GameStates lastGameplayScene = GameStates.NONE;

    /// <summary>
    /// Checks the current active scene and updates the currentScene variable accordingly.
    /// </summary>
    public static void CheckCurrentScene()
    {
        currentScene = (GameStates)SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Checks if the given scene is a gameplay scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static bool IsGameplay(GameStates scene)
    {
        return scene >= GameStates.LEVEL1 && scene <= GameStates.FINAL_LEVEL;
    }

    /// <summary>
    /// Checks the cursor visibility and lock state based on the target scene.
    /// </summary>
    /// <param name="targetScene"></param>
    private static void CheckCursor(GameStates targetScene)
    {
        if (IsGameplay(targetScene))
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

    /// <summary>
    /// Checks if the game should be reset based on the target scene and resets the game if necessary.
    /// </summary>
    /// <param name="scene"></param>
    private static void CheckReset(GameStates scene)
    {
        if (GameManager.ShouldReset(scene))
            GameManager.ResetGame();
    }

    /// <summary>
    /// Loads the specified scene and updates the current scene state.
    /// </summary>
    /// <param name="targetScene"></param>
    public static void GoToScene(GameStates targetScene)
    {
        CheckCurrentScene();
        CheckCursor(targetScene);

        previousScene = currentScene;

        if (IsGameplay(previousScene))
            lastGameplayScene = previousScene;

        CheckReset(targetScene);

        SceneManager.LoadScene((int)targetScene);

        currentScene = targetScene;
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public static void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    /// <summary>
    /// Navigates to the previous scene, or the main menu if no previous scene exists.
    /// </summary>
    public static void GoToPreviousScene()
    {
        CheckCurrentScene();

        if (previousScene == GameStates.NONE)
        {
            previousScene = GameStates.MAINMENU;
            Debug.LogWarning("No previous scene, going to main menu");
        }
        GoToScene(previousScene);
    }

    /// <summary>
    /// Navigates to the last gameplay scene, or logs a warning if no such scene exists.
    /// </summary>
    public static void GoToLastGameplayScene()
    {
        CheckCurrentScene();
        if (lastGameplayScene == GameStates.NONE)
            Debug.LogWarning("No last gameplay scene");
        else
            GoToScene(lastGameplayScene);
    }
}
