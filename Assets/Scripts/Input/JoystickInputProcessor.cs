using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInputProcessor : IInputProcessor
{
    private float _sensitivityMultiplier;
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
