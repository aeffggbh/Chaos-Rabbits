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
    public static bool IsGrounded(Collider collider)
    {
        float rayLength = 0.2f;
        Vector3 origin = collider.bounds.center;
        origin.y = collider.bounds.min.y + 0.01f;

        RaycastHit hit;
        if (Physics.Raycast(origin, -collider.transform.up, out hit, rayLength))
        {
            return true;
        }

        return false;
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

    /// <summary>
    /// Checks if an object is pointing towards another object in a specific direction
    /// </summary>
    /// <param name="start"></param>
    /// <param name="maxDistance"></param>
    /// <param name="hitInfo"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static bool PointingToObject(Transform start, float maxDistance, out RaycastHit hitInfo, Vector3 dir)
    {
        dir.Normalize();

        Ray ray = new(start.position, dir);

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 100f);

        // Perform the ray cast
        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            return true;
        }

        return false;
    }

    
}
