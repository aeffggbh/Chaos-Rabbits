/// <summary>
/// The pause menu state
/// </summary>
public class PauseMenuState : IMenuState, IExitStateCommand
{
    /// <summary>
    /// Can unpause the game
    /// </summary>
    public void ExitState()
    {
        PauseManager.Paused = false;
    }

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

