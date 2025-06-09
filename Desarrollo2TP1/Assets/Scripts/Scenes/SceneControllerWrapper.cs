using System;
using UnityEngine;
using static SceneController;
/// <summary>
/// Class to call the methods on SceneController from the UI.
/// </summary>
public class SceneControllerWrapper : MonoBehaviour
{
    public void GoTo(GameStates state)
    {
        SceneController.GoToScene(state);
    }
    public void GoToGameOver()
    {
        SceneController.GoToScene(GameStates.GAMEOVER);
    }

    public void GoToMainMenu()
    {
        SceneController.GoToScene(GameStates.MAINMENU);
    }

    public void GoToLevel1()
    {
        SceneController.GoToScene(GameStates.LEVEL1);
    }

    public void GoToLevel2()
    {
        SceneController.GoToScene(GameStates.LEVEL2);
    }

    public void GoToLevel3()
    {
        SceneController.GoToScene(GameStates.FINAL_LEVEL);
    }

    public void GoToCredits()
    {
        SceneController.GoToScene(GameStates.CREDITS);
    }
    public void CheckExit()
    {
        SceneController.GoToScene(GameStates.CHECKEXIT);
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
