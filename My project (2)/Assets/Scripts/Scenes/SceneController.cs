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
        NONE
    }

    public static Scenes currentScene = Scenes.NONE;
    //so I can go back to the previous scene if I put "no" on checkexit
    public static Scenes previousScene = Scenes.NONE;

    public static void CheckCurrentScene()
    {
        currentScene = (Scenes)SceneManager.GetActiveScene().buildIndex;
    }

    public static void GoToScene(Scenes scene)
    {
        if (scene >= Scenes.LEVEL1 && scene <= Scenes.FINAL_LEVEL)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        previousScene = currentScene;
        SceneManager.LoadScene((int)scene);

        currentScene = scene;
    }


    public static void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    public static void GoToPreviousScene()
    {
        if (previousScene == Scenes.NONE)
        {
            previousScene = Scenes.MAINMENU;
            Debug.LogWarning("No previous scene, going to main menu");
        }
        GoToScene(previousScene);
    }
}
