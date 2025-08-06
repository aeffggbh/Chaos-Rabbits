using UnityEngine;

public class EnemyPrefabRandomizer : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;

    private void Start()
    {
        _enemyFactory.CreateEnemy(transform);
    }
}
