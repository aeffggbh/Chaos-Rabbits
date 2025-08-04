using System;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInputProcessor : IInputProcessor
{
    private float _sensitivityMultiplier;
    private float _deadZone;
    private float _lastInputMagnitude;
    private AnimationCurve _joystickCurve;
    public float SensitivityMultiplier => _sensitivityMultiplier;

    public JoystickInputProcessor(float sensitivityMultiplier = 5f)
    {
        _sensitivityMultiplier = sensitivityMultiplier;
    }

    public bool CanProcessInput(InputDevice inputDevice)
    {
        return inputDevice is Gamepad || inputDevice is Joystick;
    }
}
