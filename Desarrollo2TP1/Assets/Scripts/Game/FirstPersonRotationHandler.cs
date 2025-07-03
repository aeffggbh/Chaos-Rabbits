using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonRotationHandler : MonoBehaviour
{
    [SerializeField] private GameObject _lookXGO;
    [SerializeField] private GameObject _lookYGO;
    [SerializeField] private Transform actualCamera;
    [SerializeField] private float _bodyDelay = 1f;
    private Vector3 camForward;

    private void Awake()
    {
        if (!_lookXGO)
            Debug.LogError(nameof(_lookXGO) + " is not assigned in the inspector.");
        if (!_lookYGO)
            Debug.LogError(nameof(_lookYGO) + " is not assigned in the inspector.");
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

        LookAtTarget.Look(camForward, _lookXGO.transform, LookAtTarget.AxisLock.X, _bodyDelay);
        LookAtTarget.Look(camForward, _lookYGO.transform, _bodyDelay);
    }
}
