using System;
using UnityEngine;

/// <summary>
/// Manages raycasting operations in the game.
/// </summary>
public class RayManager
{
    public bool PointingToObject(Transform start, float maxDistance, out RaycastHit hitInfo)
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
