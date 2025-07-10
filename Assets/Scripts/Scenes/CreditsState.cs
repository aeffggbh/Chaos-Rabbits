/// <summary>
/// The credits menu state
/// </summary>
public class CreditsState : IMenuState
{
    public void EnterState(MenuManager manager)
    {
        manager.ShowPanel(manager.CreditsMenuPanel);
    }
}
