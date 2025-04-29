using System;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _prefabBullet;
    [SerializeField] private Transform _tip;
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private Transform _world;
    [SerializeField] private Transform _FPCamera;
    private Vector3 defaultPos;
    private Vector3 rotation;

    private void OnEnable()
    {
        ResetPos();


        transform.SetParent(_FPCamera);

        if (_shootAction)
            _shootAction.action.started += OnShoot;
        else
            Debug.LogError(nameof(_shootAction) + " is null");
    }

    void ResetPos()
    {
        defaultPos = _FPCamera.transform.position;
        //defaultPos.x += 1;
        //defaultPos.y -= 1;
        //defaultPos.z += 2;

        defaultPos += _FPCamera.right; 
        defaultPos += -_FPCamera.up * 1.2f; 
        defaultPos += _FPCamera.forward * 2; 



        transform.position = defaultPos;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        FireInstance();
    }

    public void FireInstance()
    {
        var newBullet = Instantiate(_prefabBullet,
                 _tip.position,
                 _tip.rotation);

        newBullet.Fire();
    }

    public void Hold()
    {
        ResetPos();
        transform.eulerAngles = _FPCamera.eulerAngles;

        transform.SetParent(_FPCamera);
        if (GetComponent<Rigidbody>())
            Destroy(GetComponent<Rigidbody>());
    }

    public void Drop()
    {
        Debug.Log("drop");
        transform.SetParent(_world);
        if (!GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
    }
}
