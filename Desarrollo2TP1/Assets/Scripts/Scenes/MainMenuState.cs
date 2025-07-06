
public class MainMenuState : IMenuState
{
    public void EnterState(MenuManager manager)
    {
        if (UIAudioHandler.Instance != null)
            UIAudioHandler.Instance.ActivateAudioListener();

        GameSceneController.Instance.UnloadGameplay();

        GameManager.ResetGame();

        manager.ShowPanel(manager.MainMenuPanel);
    }
}
