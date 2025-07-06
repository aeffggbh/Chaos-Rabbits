using UnityEngine;

/// <summary>
/// A utility class that allows a GameObject to smoothly look at a target position.
/// </summary>
public static class LookAtTarget
{
    public enum AxisLock
    {
        X,
        Y,
        Z
    }

    public static float defaultSensitivity = 2f;

    public static void Look(Vector3 target, Transform origin, AxisLock axisLock, float sensitivity)
    {
        Quaternion initialRotation = origin.rotation;
        target = Lock(target, origin, axisLock);
        Quaternion lookRotation = Quaternion.LookRotation(target - origin.position);

        float time = Time.deltaTime * sensitivity;

        origin.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);
    }

    public static void Look(Vector3 target, Transform origin, float sensitivity)
    {
        Look(target, origin, AxisLock.Y, sensitivity);
    }

    public static void Look(Vector3 target, Transform origin, AxisLock axisLock)
    {
        Look(target, origin, axisLock, defaultSensitivity);
    }

    public static void Look(Vector3 target, Transform origin)
    {
        Look(target, origin, AxisLock.Y, defaultSensitivity);
    }


    private static Vector3 Lock(Vector3 target, Transform origin, AxisLock axisLock)
    {
        Vector3 auxTarget = target;

        switch (axisLock)
        {
            case AxisLock.X:
                auxTarget.x = origin.position.x;
                break;
            case AxisLock.Y:
                auxTarget.y = origin.position.y;
                break;
            case AxisLock.Z:
                auxTarget.z = origin.position.z;
                break;
            default:
                auxTarget.y = origin.position.y;
                break;
        }

        return auxTarget;
    }

}
