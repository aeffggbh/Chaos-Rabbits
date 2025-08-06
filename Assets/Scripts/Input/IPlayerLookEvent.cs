using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerLookEvent : IEvent
{
    /// <summary>
    /// Saves the direction of the input Look
    /// </summary>
    Vector2 LookDir { get;}
    /// <summary>
    /// Saves the input device that "looked"
    /// </summary>
    InputDevice InputDevice { get;}
}
