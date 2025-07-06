public class CreditsState : IMenuState
{
    public void EnterState(MenuManager manager)
    {
        manager.ShowPanel(manager.CreditsMenuPanel);
    }
}
