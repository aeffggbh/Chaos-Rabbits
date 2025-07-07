using UnityEditor;

public interface IActivateSceneEvent : IEvent
{
    int Index { get; }
    void GetIndex();
}
