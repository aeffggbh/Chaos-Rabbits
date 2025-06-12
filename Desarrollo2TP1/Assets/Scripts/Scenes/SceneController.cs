using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for managing scene transitions and game states.
/// </summary>
public static class SceneController
{
    public enum GameState
    {
        MAINMENU,
        LEVEL1,
        LEVEL2,
        FINAL_LEVEL,
        GAMEWIN,
        CHECKEXIT,
        GAMEOVER,
        CREDITS,
        EXIT,
        PREVIOUS_SCENE,
        LAST_GAMEPLAY,
        NONE
    }

    private enum GameStateType
    {
        SPECIFIC,
        UTIL,
        NONE
    }

    public static GameState currentScene = GameState.NONE;
    //so I can go back to the previous scene if I put "no" on checkexit
    public static GameState previousScene = GameState.NONE;
    public static GameState lastGameplayScene = GameState.NONE;

    /// <summary>
    /// Checks the current active scene and updates the currentScene variable accordingly.
    /// </summary>
    public static void CheckCurrentScene()
    {
        currentScene = (GameState)SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Checks if the given scene is a gameplay scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static bool IsGameplay(GameState scene)
    {
        return scene >= GameState.LEVEL1 && scene <= GameState.FINAL_LEVEL;
    }

    /// <summary>
    /// Checks the cursor visibility and lock state based on the target scene.
    /// </summary>
    /// <param name="targetScene"></param>
    private static void CheckCursor(GameState targetScene)
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
    private static void CheckReset(GameState scene)
    {
        if (GameManager.ShouldReset(scene))
            GameManager.ResetGame();
    }

    /// <summary>
    /// Loads the specified scene and updates the current scene state.
    /// </summary>
    /// <param name="targetScene"></param>
    public static void GoToScene(GameState targetScene)
    {
        GameStateType type = GetStateType(targetScene);

        if (type == GameStateType.SPECIFIC)
            CheckSpecificScene(targetScene);
        else
            CheckUtilScene(targetScene);
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

        if (previousScene == GameState.NONE)
        {
            previousScene = GameState.MAINMENU;
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
        if (lastGameplayScene == GameState.NONE)
            Debug.LogWarning("No last gameplay scene");
        else
            GoToScene(lastGameplayScene);
    }

    private static GameStateType GetStateType(GameState state)
    {
        if (state >= GameState.EXIT)
            return GameStateType.UTIL;
        else
            return GameStateType.SPECIFIC;

    }

    private static void CheckUtilScene(GameState targetScene)
    {
        switch (targetScene)
        {
            case GameState.EXIT:
                ExitGame();
                break;
            case GameState.PREVIOUS_SCENE:
                GoToPreviousScene();
                break;
            case GameState.LAST_GAMEPLAY:
                GoToLastGameplayScene();
                break;
            default:
                break;
        }
    }

    private static void CheckSpecificScene(GameState targetScene)
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
}
