
/// <summary>
/// Manages overal gameplay variables and replayability
/// </summary>
public static class GameManager
{
    /// <summary>
    /// Resets the game
    /// </summary>
    public static void ResetGame()
    {
        PlayerMediator.PlayerInstance?.CheatsController?.Reset();
        EventTriggerManager.Trigger<IDeleteUserEvent>(new DeleteUserEvent(null));
        PlayerPreservedData.Reset();
    }
}
