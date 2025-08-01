using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour, ICarInput
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _brakeAction;
    [SerializeField] private InputActionReference _steerAction;

    private bool _brakeInput;
    private float _steerInput;
    private float _moveInput;
    public float MoveInput => _moveInput;
    public float SteerInput => _steerInput;
    public bool BrakeInput => _brakeInput;

    private void Awake()
    {
        _moveAction.action.performed += OnMove;

        _steerAction.action.performed += OnSteer;
        _steerAction.action.canceled += OnSteer;

        _brakeAction.action.started += OnBrakeStarted;
        _brakeAction.action.canceled += OnBrakeCanceled;
    }

    private void OnBrakeStarted(InputAction.CallbackContext context)
    {
        _brakeInput = true;
    }

    private void OnSteer(InputAction.CallbackContext context)
    {
        _steerInput = context.ReadValue<float>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();
    }

    private void OnBrakeCanceled(InputAction.CallbackContext context)
    {
        _brakeInput = false;
        _steerInput = 0;
        _moveInput = 0;
    }
}
