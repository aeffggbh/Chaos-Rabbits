using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] public Transform respawnPos;
    //TODO: move the object list to the manager, otherwise I have to assign the objects to each wall xd
    [SerializeField] private List<GameObject> _objectsToRespawn;

    private void OnCollisionEnter(Collision collision)
    {
        CheckEnter(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckEnter(other.gameObject);
    }

    private void CheckEnter(GameObject obj)
    {
        if (!obj.GetComponent<Respawner>())
        {
            if (IsObjectInList(obj))
                obj.transform.position = respawnPos.position;
            else
                Destroy(obj);
        }
    }

    private bool IsObjectInList(GameObject obj)
    {
        //if I wanted to let many objects respawn in the same respawn point in the future, this is useful.
        for (int i = 0; i < _objectsToRespawn.Count; i++)
            if (obj == _objectsToRespawn[i])
                return true;

        return false;
    }
}
