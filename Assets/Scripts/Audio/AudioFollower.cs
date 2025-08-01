using System;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class AudioFollower : MonoBehaviour
{
    private Transform _target;

    private void Awake()
    {
        EventProvider.Subscribe<IUserSpawnedEvent>(OnUserSpawned);
        EventProvider.Subscribe<IMenuShownEvent>(OnMenuShown);
    }

    private void OnMenuShown(IMenuShownEvent menuShown)
    {
        _target = menuShown.Transform;
    }

    private void OnUserSpawned(IUserSpawnedEvent userSpawned)
    {
        _target = userSpawned.Transform;
    }

    private void LateUpdate()
    {
        if (_target != null) 
            transform.position = _target.position;
    }
}

public interface IUserSpawnedEvent : IEvent
{
    Transform Transform { get; }
}

public class UserSpawnedEvent : IUserSpawnedEvent
{
    private GameObject _triggeredByGO;
    public Transform Transform { get; }

    public GameObject TriggeredByGO => _triggeredByGO;

    public UserSpawnedEvent(GameObject triggeredByGO, Transform transform)
    {
        _triggeredByGO = triggeredByGO;
        Transform = transform;
    }
}

public interface IMenuShownEvent : IEvent
{
    Transform Transform { get; }
}

public class MenuShownEvent : IMenuShownEvent
{
    private GameObject _triggeredByGO;
    public Transform Transform { get; }

    public GameObject TriggeredByGO => _triggeredByGO;

    public MenuShownEvent(GameObject triggeredByGO, Transform transform)
    {
        _triggeredByGO = triggeredByGO;
        Transform = transform;
    }
}
