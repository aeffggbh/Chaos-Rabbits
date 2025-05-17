using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
//projectile based.
internal class Bullet : MonoBehaviour
{
    [SerializeField] private float forwardForce;
    [SerializeField] private float upForce;
    [SerializeField] private float leftForce;
    [SerializeField] private Rigidbody rb;

    Vector3 point;

    public void Fire(Transform FPCamera)
    {
        rb.AddForce(FPCamera.transform.forward * forwardForce, ForceMode.Impulse);
        rb.AddForce(FPCamera.transform.up * upForce, ForceMode.Impulse);
        rb.AddForce(-FPCamera.transform.right * leftForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().Die();
            }
            else
                Debug.LogError(nameof(collision) + " doesn't have " + nameof(Enemy) + " component");
        }

        Destroy(this.gameObject);
    }

}