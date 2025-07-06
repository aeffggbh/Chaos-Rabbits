//public static class MenuMediator
//{
//    private static IMenuState _currentState = null;
//    private static IMenuState _previousState = null;

//    public static IMenuState CurrentState { get => _currentState; set => _currentState = value; }

//    /// <summary>
//    /// Useful if you enter from another scene and want to set a menu
//    /// Does not do exit commands when hiding all the panels
//    /// </summary>
//    public static void EnterInState(IMenuState menuState)
//    {
//        _previousState = null;
//        _currentState = menuState;

//        ReadData(MenuManager.Instance);
//    }

//    /// <summary>
//    /// Update the manager with the incoming data
//    /// </summary>
//    /// <param name="manager"></param>
//    public static void ReadData(MenuManager manager)
//    {
//        manager.PreviousState = _previousState;
//        manager.CurrentState = _currentState;
//    }
//}
