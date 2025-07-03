using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private GameObject _lookX;
    [SerializeField] private GameObject _lookY;
    [SerializeField] private Transform actualCamera;
    [SerializeField] private float _bodyDelay = 1f;
    private Vector3 camForward;

    private void Awake()
    {
        if (!_lookX)
            Debug.LogError(nameof(_lookX) + " is not assigned in the inspector.");
        if (!_lookY)
            Debug.LogError(nameof(_lookY) + " is not assigned in the inspector.");
    }

    private void Start()
    {
        actualCamera = CineMachineManager.Instance.transform;
        
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        camForward = actualCamera.localPosition + actualCamera.forward;

        LookAtTarget.Look(camForward, _lookX.transform, LookAtTarget.AxisLock.X, _bodyDelay);
        LookAtTarget.Look(camForward, _lookY.transform, _bodyDelay);
    }
}
