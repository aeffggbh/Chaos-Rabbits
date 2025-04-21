using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet prefabBullet;
    [SerializeField] private Transform tip;
    [SerializeField] private InputActionReference shootAction;

    private void OnEnable()
    {
        shootAction.action.started += OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        FireInstance();
    }

    public void FireInstance()
    {
        var newBullet = Instantiate(prefabBullet,
                 tip.position,
                 tip.rotation);

        newBullet.Fire();
    }
}
