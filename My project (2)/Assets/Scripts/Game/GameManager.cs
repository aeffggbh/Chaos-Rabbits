using UnityEngine.InputSystem;

public static class GameManager
{
    //responsible for replayability and general game variables
    public static CheatsController cheatsController;
    public static bool initialized = false;
    public static PlayerController savedPlayer;
    public static Weapon defaultWeapon;
    public static InputActionReference pauseButton;
    public static bool paused = false;

    //Asks for the scene you wanna go to, and if it should reset the game or not
    public static bool ShouldReset(SceneController.Scenes targetScene)
    {
        return targetScene == SceneController.Scenes.MAINMENU ||
            SceneController.previousScene == SceneController.Scenes.GAMEOVER ||
            SceneController.previousScene == SceneController.Scenes.GAMEWIN;
    }

    public static void ResetGame()
    {
        if (initialized)
        {
            cheatsController.Reset();
            savedPlayer.currentWeapon = defaultWeapon;
            savedPlayer.ResetPlayer();

            initialized = false;
        }
    }


    public static void SetPause(bool paused)
    {
        GameManager.paused = paused;
    }
}
