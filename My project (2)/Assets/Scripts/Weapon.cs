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
        if (shootAction)
        {
            shootAction.action.started += OnShoot;
        }
        else
        {
            Debug.LogError(nameof(shootAction) + " is null");
        }
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
