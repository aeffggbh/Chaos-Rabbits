public class GameOverState : IMenuState
{
    public void EnterState(MenuManager manager)
    {
        manager.ShowPanel(manager.GameOverPanel);
        GameManager.ResetGame();
        GameSceneController.Instance.UnloadGameplay();
    }

}
