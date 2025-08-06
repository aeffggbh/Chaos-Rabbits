using UnityEngine;

internal class EnemySpawnEvent : IEnemySpawnEvent
{
    private Enemy _enemy;
    private GameObject _triggeredByGO;
    private IEnemyContainer _enemyManager;
    public Enemy Enemy => _enemy;

    public GameObject TriggeredByGO => _triggeredByGO;

    public IEnemyContainer EnemyManager => _enemyManager;

    public EnemySpawnEvent(Enemy enemy, IEnemyContainer manager, GameObject triggeredBy)
    {
        _enemy = enemy;
        _triggeredByGO = triggeredBy;
        _enemyManager = manager;
    }

}