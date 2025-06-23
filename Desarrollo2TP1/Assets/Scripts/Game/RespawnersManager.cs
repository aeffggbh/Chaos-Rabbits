using System.Collections.Generic;
using UnityEngine;

public class RespawnersManager : MonoBehaviour
{
    [SerializeField] private List<Respawner> _respawnerList;
    [SerializeField] private Transform _respawnPoint;

    private void Awake()
    {
        if (_respawnPoint == null)
            Debug.LogError(nameof(RespawnersManager) + " has no respawn point assigned. Please assign one in the inspector.");
        else
            AssignSpawnPoint();
    }

    private void AssignSpawnPoint()
    {
        for (int i = 0; i < _respawnerList.Count; i++)
            _respawnerList[i].respawnPos = _respawnPoint;
    }
}
