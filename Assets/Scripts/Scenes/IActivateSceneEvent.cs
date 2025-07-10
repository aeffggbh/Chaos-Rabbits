/// <summary>
/// Event that triggers when a scene is activated
/// </summary>
public interface IActivateSceneEvent : IEvent
{
    /// <summary>
    /// Saves the index of the scene
    /// </summary>
    int Index { get; }
}
