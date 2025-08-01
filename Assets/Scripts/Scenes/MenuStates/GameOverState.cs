/// <summary>
/// The game over menu state
/// </summary>
public class GameOverState : IMenuState
{
    /// <summary>
    /// Resets the game
    /// </summary>
    /// <param name="manager"></param>
    public void EnterState(MenuManager manager)
    {
        MenuManager.ResetGame();
        manager.ShowPanel(manager.GameOverPanel);
    }
}
