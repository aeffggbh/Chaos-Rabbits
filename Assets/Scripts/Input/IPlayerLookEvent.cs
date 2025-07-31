using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerLookEvent : IEvent
{
    Vector2 LookDir { get;}
    InputDevice InputDevice { get;}
}
