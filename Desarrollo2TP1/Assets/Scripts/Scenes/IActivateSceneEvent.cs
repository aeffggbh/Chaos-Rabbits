using UnityEngine;
//TODO: make sure all interfaces have an explicit encapsulation (public, private or protected)

public interface IActivateSceneEvent : IEvent
{
    public IScene.Index SceneIndex { get; }
}
