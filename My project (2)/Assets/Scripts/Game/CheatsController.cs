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

    public static CheatsController instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);
    }

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
        SceneController.GoToScene(SceneController.currentScene + 1);
    }

    public void Reset()
    {
        _isGodMode = false;
        _isFlashMode = false;
    }
}
    