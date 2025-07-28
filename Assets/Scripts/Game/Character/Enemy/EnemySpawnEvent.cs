using UnityEngine;

internal class EnemySpawnEvent : IEnemySpawnEvent
{
    private Enemy _enemy;
    private GameObject _triggeredByGO;
    private IEnemyManager _enemyManager;
    public Enemy Enemy => _enemy;

    public GameObject TriggeredByGO => _triggeredByGO;

    public IEnemyManager EnemyManager => _enemyManager;

    public EnemySpawnEvent(Enemy enemy, IEnemyManager manager, GameObject triggeredBy)
    {
        _enemy = enemy;
        _triggeredByGO = triggeredBy;
        _enemyManager = manager;
    }

}