using UnityEngine;

/// <summary>
/// A utility class that allows a GameObject to smoothly look at a target position.
/// </summary>
public static class LookAtTarget
{
    public static float speed = 2f;

    public static void Look(Vector3 target, Transform origin)
    {
        Quaternion initialRotation = origin.rotation;
        target.y = origin.position.y; 
        Quaternion lookRotation = Quaternion.LookRotation(target - origin.position);
        float time = Time.deltaTime * speed;

        origin.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

        return;
    }

}
