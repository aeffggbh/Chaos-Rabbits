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

    public JoystickInputProcessor(AnimationCurve animationCurve = null, float deadZone = 0.2f, float sensitivityMultiplier = 5f)
    {
        _joystickCurve = new AnimationCurve();
        if (animationCurve != null && animationCurve.length > 0)
        {
            _joystickCurve.keys = animationCurve.keys;
        }
        else
        {
            _joystickCurve = new AnimationCurve(
                new Keyframe(0, 1),
                new Keyframe(1, 1)
                );
        }

        _sensitivityMultiplier = sensitivityMultiplier;
        _deadZone = deadZone;

        Debug.Log("Length: " + _joystickCurve.length.ToString());
    }

    public Vector2 ProcessInput(Vector2 rawInput)
    {
        if (rawInput.magnitude < _deadZone)
            return Vector2.zero;

        Debug.Log(rawInput);

        Vector2 dir = rawInput.normalized;

        return dir * _sensitivityMultiplier;
    }

    public bool CanProcessInput(InputDevice inputDevice)
    {
        return inputDevice is Gamepad || inputDevice is Joystick;
    }
}
