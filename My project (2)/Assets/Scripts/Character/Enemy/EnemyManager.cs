using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "EnemyManager", menuName = "ScriptableObjects/EnemyManager")]
public class EnemyManager : MonoBehaviour
{
    [SerializeField] public List<Enemy> enemies;
    [SerializeField] public PlayerController playerController;
    [SerializeField] public float attackTimer;
    [SerializeField] public float walkRange;
    [SerializeField] public float attackRange;
    [SerializeField] public float chaseRange;
    [SerializeField] public float chasingSpeed;
    [SerializeField] public float patrolSpeed;
    [SerializeField] public float patrolTimer;
    [SerializeField] public float counterMovementForce;

    private void Start()
    {
        playerController = GameObject.Find("player").GetComponent<PlayerController>();
        if (!playerController)
            Debug.LogError(nameof(playerController) + " missing");
        if (enemies.Count == 0)
            Debug.LogWarning("No enemies added!");
    }
}
