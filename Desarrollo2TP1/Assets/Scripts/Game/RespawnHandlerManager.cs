using System.Collections.Generic;
using UnityEngine;

public class RespawnHandlerManager : MonoBehaviour
{
    [SerializeField] private List<RespawnLimit> _handlersList;
    [SerializeField] private Transform _respawnPoint;

    private void Awake()
    {
        if (_respawnPoint == null)
            Debug.LogError(nameof(RespawnHandlerManager) + " has no respawn point assigned. Please assign one in the inspector.");
        else
            AssignSpawnPoint();
    }

    private void AssignSpawnPoint()
    {
        if (_respawnPoint != null)
            for (int i = 0; i < _handlersList.Count; i++)
                if (_handlersList[i] != null)
                    _handlersList[i].respawnPos = _respawnPoint;
    }
}
