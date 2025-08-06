using UnityEngine;

internal class EnemyDespawnEvent : IEnemyDespawnEvent
{
    private Enemy _enemy;
    private IEnemyContainer _enemyManager;
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;
    public Enemy Enemy => _enemy;
    public IEnemyContainer EnemyManager => _enemyManager;

    public EnemyDespawnEvent(Enemy enemy, IEnemyContainer manager, GameObject gameObject)
    {
        _enemy = enemy;
        _enemyManager = manager;
        _triggeredByGO = gameObject;
    }

}