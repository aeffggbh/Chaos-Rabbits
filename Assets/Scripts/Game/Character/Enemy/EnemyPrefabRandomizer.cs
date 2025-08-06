using UnityEngine;

public class EnemyPrefabRandomizer : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private Enemy _enemy;

    private void Start()
    {
        _enemy = _enemyFactory.CreateEnemy(transform);
    }

    private void OnDisable()
    {
        _enemy?.Die();
    }
}
