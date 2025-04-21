using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
internal class Bullet : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private Rigidbody rb;

    //public void SetData(BulletModel model)
    //{

    //}
    public void Fire()
    {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: detect if it's an enemy
        Destroy(this.gameObject);
    }
}