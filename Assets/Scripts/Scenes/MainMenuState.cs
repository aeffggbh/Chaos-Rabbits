
/// <summary>
/// The menu state
/// </summary>
public class MainMenuState : IMenuState
{
    /// <summary>
    /// Resets the game.
    /// </summary>
    /// <param name="manager"></param>
    public void EnterState(MenuManager manager)
    {
        MenuManager.ResetGame();

        manager.ShowPanel(manager.MainMenuPanel);
    }
}
