public class GameWinState : IMenuState
{
    public void EnterState(MenuManager manager)
    {
        GameManager.ResetGame();
        GameSceneController.Instance.UnloadGameplay();
        manager.ShowPanel(manager.GameWinPanel);
    }
}


    