/// <summary>
/// The game win menu state
/// </summary>
public class GameWinState : IMenuState
{
    /// <summary>
    /// Resets the game
    /// </summary>
    /// <param name="manager"></param>
    public void EnterState(MenuManager manager)
    {
        MenuManager.ResetGame();
        manager.ShowPanel(manager.GameWinPanel);
    }
}


    