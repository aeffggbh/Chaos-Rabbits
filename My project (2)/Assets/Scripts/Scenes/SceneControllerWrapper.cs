using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneController;

//For buttons
public class SceneControllerWrapper : MonoBehaviour
{
    public void GoToGameOver()
    {
        SceneController.GoToScene(Scenes.GAMEOVER);
    }

    public void GoToMainMenu()
    {
        SceneController.GoToScene(Scenes.MAINMENU);
    }

    public void GoToLevel1()
    {
        SceneController.GoToScene(Scenes.LEVEL1);
    }

    public void GoToLevel2()
    {
        SceneController.GoToScene(Scenes.LEVEL2);
    }

    public void GoToLevel3()
    {
        SceneController.GoToScene(Scenes.FINAL_LEVEL);
    }

    public void GoToCredits()
    {
        SceneController.GoToScene(Scenes.CREDITS);
    }
    public void CheckExit()
    {
        SceneController.GoToScene(Scenes.CHECKEXIT);
    }
    public void ExitGame()
    {
        SceneController.ExitGame();
    }

    public void GoToPreviousScene()
    {
        SceneController.GoToPreviousScene();
    }
}
