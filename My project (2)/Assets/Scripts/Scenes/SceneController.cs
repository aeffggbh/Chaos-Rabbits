using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public enum Scenes
    {
        MAINMENU,
        LEVEL1,
        LEVEL2,
        FINAL_LEVEL,
        CHECKEXIT,
        GAMEOVER,
        CREDITS,
        GAMEWIN,
        NONE
    }

    public static Scenes currentScene = Scenes.NONE;
    //so I can go back to the previous scene if I put "no" on checkexit
    public static Scenes previousScene = Scenes.NONE;
    public static Scenes lastGameplayScene = Scenes.NONE;

    public static void CheckCurrentScene()
    {
        currentScene = (Scenes)SceneManager.GetActiveScene().buildIndex;
    }

    public static bool IsGameplay(Scenes scene)
    {
        return scene >= Scenes.LEVEL1 && scene <= Scenes.FINAL_LEVEL;
    }

    private static void CheckCursor(Scenes targetScene)
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

    private static void CheckReset(Scenes scene)
    {
        if (GameManager.ShouldReset(scene))
            GameManager.ResetGame();
        //else if (!IsGameplay(scene))
            //GameManager.PauseGame();
    }

    public static void GoToScene(Scenes targetScene)
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


    public static void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    public static void GoToPreviousScene()
    {
        CheckCurrentScene();

        if (previousScene == Scenes.NONE)
        {
            previousScene = Scenes.MAINMENU;
            Debug.LogWarning("No previous scene, going to main menu");
        }
        GoToScene(previousScene);
    }

    public static void GoToLastGameplayScene()
    {
        CheckCurrentScene();
        if (lastGameplayScene == Scenes.NONE)
            Debug.LogWarning("No last gameplay scene");
        else
            GoToScene(lastGameplayScene);
    }
}
