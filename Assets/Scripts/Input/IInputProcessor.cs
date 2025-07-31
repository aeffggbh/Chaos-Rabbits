using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputProcessor
{
    Vector2 ProcessInput(Vector2 rawInput);
    bool CanProcessInput(InputDevice inputDevice);
    float SensitivityMultiplier { get; }
}
