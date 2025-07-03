using System.Collections.Generic;
using UnityEngine;

public class RespawnHandlerManager : MonoBehaviour
{
    [SerializeField] private List<RespawnHandler> _handlersList;
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
        for (int i = 0; i < _handlersList.Count; i++)
            _handlersList[i].respawnPos = _respawnPoint;
    }
}
