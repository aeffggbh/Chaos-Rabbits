using System;
using UnityEngine;

/// <summary>
/// Manages raycasting operations in the game.
/// </summary>
public static class RayManager
{
    public static bool IsGrounded(Transform transform, float groundDistance)
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundDistance);
    }

    public static bool PointingToObject(Transform start, float maxDistance, out RaycastHit hitInfo)
    {
        Ray ray = new(start.position, start.forward);

        // Perform the raycast
        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            return true;
        }

        return false;
    }
}
