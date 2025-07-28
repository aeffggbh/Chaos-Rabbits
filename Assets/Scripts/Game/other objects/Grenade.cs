using UnityEngine;

public class Grenade : MonoBehaviour, IGrenade
{
    [SerializeField] float throwForce;

    public void Throw()
    {
        var rb = gameObject.AddComponent<Rigidbody>();

        Vector3 force = transform.forward * throwForce + transform.up * throwForce/2;

        rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
    }


}
