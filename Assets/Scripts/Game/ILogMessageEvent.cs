public interface ILogMessageEvent : IEvent
{
    string Text { get; }
    bool IsToggle { get; }
    bool Toggle { get; }
}