using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatsController : MonoBehaviour
{
    [SerializeField] private InputActionReference _nextLevel;
    [SerializeField] private InputActionReference _godMode;
    [SerializeField] private InputActionReference _flashMode;
    public bool _isGodMode;
    public bool _isFlashMode;

    private void OnEnable()
    {
        if (!_nextLevel)
            Debug.LogError(nameof(_nextLevel) + " is null");
        else
            _nextLevel.action.performed += OnNextLevel;

        if (!_godMode)
            Debug.LogError(nameof(_godMode) + " is null");
        else
            _godMode.action.performed += OnGodMode;

        if (!_flashMode)
            Debug.LogError(nameof(_flashMode) + " is null");
        else
            _flashMode.action.performed += OnFlashMode;
    }

    private void OnFlashMode(InputAction.CallbackContext context)
    {
        _isFlashMode = !_isFlashMode;
    }

    private void OnGodMode(InputAction.CallbackContext context)
    {
        _isGodMode = !_isGodMode;
    }

    private void OnNextLevel(InputAction.CallbackContext context)
    {
        //TODO: Implement the logic to go to the next level
    }
}
