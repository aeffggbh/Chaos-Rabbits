using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the character, controls everything related to the world and the position.
/// </summary>
/// 
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    //private HashSet<ForceRequest> instantForceRequests;

    //private HashSet<ForceRequest> continuousForceRequests;
    private ForceRequest instantForceRequests;
    private ForceRequest continuousForceRequests;
    private Rigidbody rb;

    public void RequestInstantForce(ForceRequest forceRequest)
    {
        //instantForceRequests.Add(forceRequest);
        instantForceRequests = forceRequest;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (continuousForceRequests != null /*&& continuousForceRequests.Count > 0*/)
        {
            var speedPercentage = rb.linearVelocity.magnitude / continuousForceRequests.speed;
            //limitar el valor entre 0 y 1
            var remainingSpeedPercentage = Mathf.Clamp01(1f - speedPercentage);

            //if (rb.linearVelocity.magnitude < continuousForceRequests.speed)
            //El .force se va a multiplicar por fixeddelta internamente. el .impulse no.
            rb.AddForce(continuousForceRequests.direction * continuousForceRequests.acceleration * remainingSpeedPercentage, ForceMode.Force);
        }

        if (instantForceRequests == null)
            return;


        rb.AddForce(instantForceRequests.direction * instantForceRequests.acceleration, instantForceRequests.forceMode);
        instantForceRequests = null;
    }
}
