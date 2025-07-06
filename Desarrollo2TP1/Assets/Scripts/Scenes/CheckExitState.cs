public class CheckExitState : IMenuState
{
    public void EnterState(MenuManager manager)
    {
        manager.ShowPanel(manager.CheckExitMenuPanel);
    }

}
