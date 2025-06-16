using System;
using UnityEngine;

/// <summary>
/// Manages raycasting operations in the game.
/// </summary>
public class RayManager
{
    private float _distanceToObject;

    public RayManager()
    {
        _distanceToObject = 0f;
    }
    public bool PointingToObject(Transform start, float maxDistance, out RaycastHit hitInfo)
    {
        Ray ray = new Ray(start.position, start.forward);

        // Perform the raycast
        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// Calculates the distance of the object
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public float GetDistanceToObject(Transform start, Transform end)
    {
        float diffX = end.position.x - start.position.x;
        float diffY = end.position.y - start.position.y;
        float diffZ = end.position.z - start.position.z;
        _distanceToObject = (float)Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ);
        return _distanceToObject;
    }

    public float GetDistanceToObject()
    {
        if (_distanceToObject< 1)
        {
            Debug.LogError(nameof(GetDistanceToObject) + ": Distance was not defined or is too small");
            return 0f;
        }

        return _distanceToObject;
    }

}
