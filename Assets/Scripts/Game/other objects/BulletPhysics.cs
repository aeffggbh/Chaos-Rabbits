using System;
using UnityEngine;

/// <summary>
/// Handles the physics of the bullet
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BulletPhysics : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Fires the bullet
    /// </summary>
    /// <param name="wParent"></param>
    public void Fire(Transform wParent)
    {
        _rb.AddForce(_force * Time.fixedDeltaTime * wParent.forward, ForceMode.Impulse);
    }
}