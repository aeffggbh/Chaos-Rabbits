using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputProcessor
{
    bool CanProcessInput(InputDevice inputDevice);
    float SensitivityMultiplier { get; }
}
