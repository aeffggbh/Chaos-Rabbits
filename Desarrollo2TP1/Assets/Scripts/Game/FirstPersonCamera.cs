using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private GameObject _lookX;
    [SerializeField] private GameObject _lookY;
    //[SerializeField] private InputActionReference _lookAction;
    [SerializeField] private Transform actualCamera;
    [SerializeField] private float _sensitivity = 1f;

    private void Awake()
    {
        if (!_lookX)
            Debug.LogError(nameof(_lookX) + " is not assigned in the inspector.");
        if (!_lookY)
            Debug.LogError(nameof(_lookY) + " is not assigned in the inspector.");
        //if (!_lookAction)
        //    Debug.LogWarning(nameof(_lookAction) + " is not assigned in the inspector.");


    }

    private void Start()
    {
        actualCamera = CinemachineManager.instance.transform;
        
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        //Vector2 mouseLook = _lookAction.action.ReadValue<Vector2>();
        //Vector2 mouseLook = actualCamera.transform.localRotation;

        //float mouseX = mouseLook.x * _sensitivity * Time.deltaTime;
        //float mouseY = mouseLook.y * _sensitivity * Time.deltaTime;

        //float xRotation = mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //_lookX.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //_lookY.transform.Rotate(Vector3.up * mouseX);

        LookAtTarget.Look(actualCamera.localPosition + actualCamera.forward, _lookX.transform, LookAtTarget.AxisLock.X, _sensitivity);
        LookAtTarget.Look(actualCamera.localPosition + actualCamera.forward, _lookY.transform, _sensitivity);

    }
}
