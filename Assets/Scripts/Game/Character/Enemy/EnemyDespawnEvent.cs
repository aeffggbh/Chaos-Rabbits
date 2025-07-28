using UnityEngine;

internal class EnemyDespawnEvent : IEnemyDespawnEvent
{
    private Enemy _enemy;
    private IEnemyManager _enemyManager;
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;
    public Enemy Enemy => _enemy;
    public IEnemyManager EnemyManager => _enemyManager;

    public EnemyDespawnEvent(Enemy enemy, IEnemyManager manager, GameObject gameObject)
    {
        _enemy = enemy;
        _enemyManager = manager;
        _triggeredByGO = gameObject;
    }

}