/// <summary>
/// The check exit menu state
/// </summary>
public class CheckExitState : IMenuState
{
    public void EnterState(MenuManager manager)
    {
        manager.ShowPanel(manager.CheckExitMenuPanel);
    }
}
