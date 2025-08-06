using System.Collections.Generic;
using UnityEngine;

//TODO: adjust this

/// <summary>
/// Manages all enemies in the game.
/// </summary>
public class EnemyContainer : MonoBehaviour, IEnemyContainer
{
    [SerializeField] private List<Enemy> enemies;
    public List<Enemy> Enemies => enemies;

    private void Awake()
    {
        ServiceProvider.SetService<IEnemyContainer>(this, true);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<IEnemyContainer>(null, true);
    }
        
    private void Start()
    {
        if (enemies.Count == 0)
            Debug.LogWarning("No enemies added!");
    }

    public void Add(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void Remove(Enemy enemy)
    {
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);
    }
}
