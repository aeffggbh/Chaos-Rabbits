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
    [SerializeField] private float distance;

    Vector3 point;

    public void Fire(Transform FPCamera)
    {
        //if (distance < 1)
        //{
        //    Debug.LogError("distance is too small");
        //    return;
        //}

        ////How do I make it go to the center? This is not good enough
        //point = FPCamera.transform.position + FPCamera.transform.forward * distance;

        //Vector3 direction = (point - transform.position).normalized;

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
                Debug.Log("Died with bullet");
                collision.gameObject.GetComponent<Enemy>().Die();
            }
            else
                Debug.LogError(nameof(collision) + " doesn't have " + nameof(Enemy) + " component");
        }

        Destroy(this.gameObject);
    }

}