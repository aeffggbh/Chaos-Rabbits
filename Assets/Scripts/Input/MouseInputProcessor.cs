using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The mouse input processor
/// </summary>
public class MouseInputProcessor : IInputProcessor
{
    private float _sensitivityMultiplier;
    private float _lastInputMagnitude;
    public float SensitivityMultiplier => _sensitivityMultiplier;

    public MouseInputProcessor(float sensitivityMultiplier = 0.5f)
    {
        _sensitivityMultiplier = sensitivityMultiplier;
    }
    public bool CanProcessInput(InputDevice inputDevice)
    {
        return inputDevice is Mouse;
    }
}
