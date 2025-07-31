using NUnit.Framework;
using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class SensitivityHandler : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private float _baseSens = 1f;

    [Header("Mouse Settings")]
    [SerializeField] private float _mouseMultiplier = 0.5f;

    [Header("Joystick Settings")]
    [SerializeField] private float _joystickMultiplier = 5f;
    [SerializeField] private float _joystickDeadZone = 0.2f;
    [SerializeField] private AnimationCurve _joystickResponseCurve = AnimationCurve.Linear(0,0,1,1);

    private CinemachineCamera _camera;
    private CinemachinePanTilt _panTilt;
    private InputManager _inputManager;

    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
        if (!_camera)
            Debug.LogError("No virtual camera found");

        _panTilt = GetComponent<CinemachinePanTilt>();
        if (!_panTilt)
            Debug.LogError("No pan tilt found");

        InitInputManager();
    }

    private void InitInputManager()
    {
        _inputManager = new InputManager();

        _inputManager.RegisterProcessor(new MouseInputProcessor(_mouseMultiplier));
        _inputManager.RegisterProcessor(new JoystickInputProcessor(
            _joystickResponseCurve,
            _joystickDeadZone,
            _joystickMultiplier));
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

        var processedInput = processor.ProcessInput(playerLook.LookDir) * _baseSens;

        ApplyCameraRotation(processedInput);
    }

    private void ApplyCameraRotation(Vector2 lookInput)
    {
        if (lookInput.magnitude < 0.1f)
            return;

        var panTilt = _camera.GetComponent<CinemachinePanTilt>();

        if (panTilt)
        {
            panTilt.PanAxis.Value += lookInput.x * Time.deltaTime;
            panTilt.TiltAxis.Value -= lookInput.y * Time.deltaTime;
        }
    }
}
