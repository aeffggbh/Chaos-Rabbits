using System;
using UnityEngine;

/// <summary>
/// Manages ray casting operations in the game.
/// </summary>
public static class RayManager
{
    /// <summary>
    /// Checks if a given transform is grounded
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="groundDistance"></param>
    /// <returns></returns>
    public static bool IsGrounded(Transform transform, float groundDistance)
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundDistance);
    }

    /// <summary>
    /// Checks if I'm pointing towards an object
    /// </summary>
    /// <param name="start"></param>
    /// <param name="maxDistance"></param>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    public static bool PointingToObject(Transform start, float maxDistance, out RaycastHit hitInfo)
    {
        Ray ray = new(start.position, start.forward);

        // Perform the ray cast
        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            return true;
        }

        return false;
    }
}
