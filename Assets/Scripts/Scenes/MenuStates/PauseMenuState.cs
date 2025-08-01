/// <summary>
/// The pause menu state
/// </summary>
public class PauseMenuState : IMenuState
{
    /// <summary>
    /// Can pause the game
    /// </summary>
    /// <param name="manager"></param>
    public void EnterState(MenuManager manager)
    {
        PauseManager.Paused = true;
        manager.ShowPanel(manager.PauseMenuPanel);
    }
}

