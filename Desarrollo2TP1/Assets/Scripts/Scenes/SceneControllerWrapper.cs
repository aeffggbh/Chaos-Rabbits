using System;
using UnityEngine;
using static SceneController;
/// <summary>
/// Class to call the methods on SceneController from the UI.
/// </summary>
public class SceneControllerWrapper : MonoBehaviour
{
    public void GoTo(GameState state)
    {
        SceneController.GoToScene(state);
    }
    public void GoToGameOver()
    {
        SceneController.GoToScene(GameState.GAMEOVER);
    }

    public void GoToMainMenu()
    {
        SceneController.GoToScene(GameState.MAINMENU);
    }

    public void GoToLevel1()
    {
        SceneController.GoToScene(GameState.LEVEL1);
    }

    public void GoToLevel2()
    {
        SceneController.GoToScene(GameState.LEVEL2);
    }

    public void GoToLevel3()
    {
        SceneController.GoToScene(GameState.FINAL_LEVEL);
    }

    public void GoToCredits()
    {
        SceneController.GoToScene(GameState.CREDITS);
    }
    public void CheckExit()
    {
        SceneController.GoToScene(GameState.CHECKEXIT);
    }
    public void ExitGame()
    {
        SceneController.ExitGame();
    }

    public void GoToPreviousScene()
    {
        SceneController.GoToPreviousScene();
    }

    public void GoToLastGameplayScene()
    {
        SceneController.GoToLastGameplayScene();
    }
}
