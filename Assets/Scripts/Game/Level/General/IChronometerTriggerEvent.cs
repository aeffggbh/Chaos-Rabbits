using UnityEngine;

public interface IChronometerTriggerEvent : IEvent
{
    Collider Other {  get; }
}