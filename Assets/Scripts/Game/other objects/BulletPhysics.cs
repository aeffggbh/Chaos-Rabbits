using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletPhysics : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Fire(Transform wParent)
    {
        _rb.AddForce(_force * Time.fixedDeltaTime * wParent.forward, ForceMode.Impulse);
    }
}