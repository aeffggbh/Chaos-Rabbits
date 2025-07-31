public interface INewLevelEvent : IEvent
{
    int PreviousLevelIndex { get; }
}