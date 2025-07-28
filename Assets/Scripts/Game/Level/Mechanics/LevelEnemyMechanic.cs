using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMechanic", menuName = "ScriptableObjects/LevelMechanics/EnemyMechanic")]
public class LevelEnemyMechanic : LevelMechanicSO, ILevelEnemiesData, IMechanicTextInfo, ILevelMechanicInitialize
{
    private int _enemiesCount;
    private int _totalEnemies;

    public int EnemiesCount { get => _enemiesCount; set => _enemiesCount = value; }
    public int TotalEnemies { get => _totalEnemies; set => _totalEnemies = value; }
    public override bool ObjectiveCompleted => _enemiesCount >= _totalEnemies;

    public void Init()
    {
        _totalEnemies = 0;
        _enemiesCount = 0;

        EventProvider.Subscribe<IEnemyDespawnEvent>(OnEnemyDespawn);
        EventProvider.Subscribe<IEnemySpawnEvent>(OnEnemySpawn);
    }

    private void OnEnemySpawn(IEnemySpawnEvent enemy)
    {
        _totalEnemies = enemy.EnemyManager.Enemies.Count;
    }

    private void OnEnemyDespawn(IEnemyDespawnEvent enemy)
    {
        if (_totalEnemies == 0)
            _totalEnemies = enemy.EnemyManager.Enemies.Count;

        _enemiesCount++;
        if (_enemiesCount > _totalEnemies)
            _enemiesCount = _totalEnemies;
    }

    public string GetObjectiveText()
    {
        string text;

        text = "EXTERMINATE THE ";

        if (_totalEnemies > 1)
            text += "ENEMIES";
        else
            text += "ENEMY";

        text += $" {_enemiesCount}/{_totalEnemies}";

        return text;
    }
}
