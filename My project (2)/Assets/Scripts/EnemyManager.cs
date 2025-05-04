using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public List<Enemy> _enemies;
    [SerializeField] public PlayerController _playerController;
    [SerializeField] public float _attackTimer;
    [SerializeField] public float _walkRange;
    [SerializeField] public float _attackRange;
    [SerializeField] public float _chaseRange;
    [SerializeField] public float _chasingSpeed;

    private void Awake()
    {
        if (!_playerController)
            Debug.LogError(nameof(_playerController) + " missing");
    }

    private void Start()
    {
        if (_enemies.Count == 0)
            Debug.LogWarning("No enemies added!");
    }
}
