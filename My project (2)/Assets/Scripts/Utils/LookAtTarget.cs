using UnityEngine;

/// <summary>
/// A utility class that allows a GameObject to smoothly look at a target position.
/// </summary>
public class LookAtTarget : MonoBehaviour
{
    public float speed = 2f;

    public void Look(Vector3 target)
    {
        Quaternion initialRotation = transform.rotation;
        target.y = transform.position.y; 
        Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);
        float time = Time.deltaTime * speed;

        transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

        return;
    }

}
