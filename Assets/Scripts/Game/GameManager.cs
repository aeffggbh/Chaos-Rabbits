
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
        ServiceProvider.TryGetService<PlayerMediator>(out var mediator);
        mediator?.CheatsController?.Reset();
        EventTriggerer.Trigger<IDeleteUserEvent>(new DeleteUserEvent(null));
        PlayerPreservedData.Reset();
    }
}
