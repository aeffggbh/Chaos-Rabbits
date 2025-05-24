using System;
using UnityEngine;

public class RayManager
{
    private float _distanceToObject;

    public RayManager()
    {
        _distanceToObject = 0f;
    }

    //TODO: cambiarlo a raycast
    //public bool PointingToObject(Transform start, Transform end, BoxCollider endCollider)
    //{
    //    Ray _ray = new()
    //    {
    //        direction = start.forward,
    //        origin = start.position
    //    };

    //    distanceToObject = GetDistanceToObject(start, end);

    //    Vector3 pointInView = _ray.origin + (_ray.direction * distanceToObject);

    //    if (endCollider == null)
    //        Debug.LogError("Pointing to object: No collider found");

    //    Vector3 max = endCollider.bounds.max;
    //    Vector3 min = endCollider.bounds.min;

    //    return (pointInView.x >= min.x && pointInView.x <= max.x &&
    //            pointInView.y >= min.y && pointInView.y <= max.y &&
    //            pointInView.z >= min.z && pointInView.z <= max.z);
    //} 
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

    public bool PointingToObject(Transform start, Transform end, Collider endCollider)
    {
        _distanceToObject = Vector3.Distance(start.position, end.position);

        if (PointingToObject(start, _distanceToObject, out RaycastHit hit))
            if (hit.collider == endCollider)
                return true;

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
