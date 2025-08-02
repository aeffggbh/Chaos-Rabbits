using UnityEngine;

public class FirstPersonRotationHandler : MonoBehaviour
{
    [SerializeField] private GameObject _lookXGO;
    [SerializeField] private GameObject _lookYGO;
    [SerializeField] private Transform actualCamera;
    [SerializeField] private float _speed = 1f;
    private Vector3 camForward;

    private void Awake()
    {
        if (!_lookXGO)
            Debug.LogError(nameof(_lookXGO) + " is not assigned in the inspector.");
        if (!_lookYGO)
            Debug.LogError(nameof(_lookYGO) + " is not assigned in the inspector.");
    }
    
    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        camForward = actualCamera.position + actualCamera.forward * 5f;

        LookAtTarget.Look(camForward, _lookXGO.transform, LookAtTarget.AxisLock.X, _speed);
        LookAtTarget.Look(camForward, _lookYGO.transform, _speed);
    }
}
