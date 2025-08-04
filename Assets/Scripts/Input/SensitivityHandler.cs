using NUnit.Framework;
using System;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineInputAxisController))]
public class SensitivityHandler : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private float _baseSens = 1f;

    [Header("Mouse Settings")]
    [SerializeField] private float _mouseMultiplier = 0.5f;

    [Header("Joystick Settings")]
    [SerializeField] private float _joystickMultiplier = 5f;

    private CinemachineInputAxisController _axisControl;
    private InputManager _inputManager;

    private void Awake()
    {
        _axisControl = GetComponent<CinemachineInputAxisController>();

        InitInputManager();
    }

    private void InitInputManager()
    {
        _inputManager = new InputManager();

        _inputManager.RegisterProcessor(new MouseInputProcessor(_mouseMultiplier));
        _inputManager.RegisterProcessor(new JoystickInputProcessor(_joystickMultiplier));
    }

    private void Start()
    {
        EventProvider.Subscribe<IPlayerLookEvent>(OnPlayerLook);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IPlayerLookEvent>(OnPlayerLook);
    }

    private void OnPlayerLook(IPlayerLookEvent playerLook)
    {
        var processor = _inputManager.GetProcessorForDevice(playerLook.InputDevice);

        float multiplier = processor.SensitivityMultiplier * _baseSens;

        foreach (var c in _axisControl.Controllers)
        {
            var sign = Math.Sign(c.Input.Gain);
            c.Input.Gain = multiplier * sign;
        }
    }
}
