
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private EnemyStats _stats;

    public EnemyStats Stats => _stats;
    public GameObject Prefab => _prefab;
}
