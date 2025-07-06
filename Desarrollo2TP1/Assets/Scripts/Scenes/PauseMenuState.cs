public class PauseMenuState : IMenuState, IExitStateCommand
{
    public void ExitState()
    {
        PauseManager.Paused = false;
    }

    public void EnterState(MenuManager manager)
    {
        PauseManager.Paused = true;
        manager.ShowPanel(manager.PauseMenuPanel);
    }
}

