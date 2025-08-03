using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHookInput : MonoBehaviour
{
    private InputActionReference _grappleAction;
    private GrapplingHook _grapplingHook;
    public GrapplingHook GrapplingHook { get => _grapplingHook; set => _grapplingHook = value; }
    public InputActionReference GrappleAction { get => _grappleAction; set => _grappleAction = value; }

    public void Init()
    {
        if (_grappleAction)
        {
            _grappleAction.action.started += _grapplingHook.OnGrapple;
            _grappleAction.action.canceled += _grapplingHook.OnGrappleCancel;
        }
    }

    private void OnDisable()
    {
        if (_grappleAction)
        {
            _grappleAction.action.started -= _grapplingHook.OnGrapple;
            _grappleAction.action.canceled -= _grapplingHook.OnGrappleCancel;
        }
    }
}