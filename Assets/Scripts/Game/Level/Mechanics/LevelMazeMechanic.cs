using UnityEngine;

[CreateAssetMenu(fileName = "MazeMechanic", menuName = "ScriptableObjects/LevelMechanics/MazeMechanic")]
public class LevelMazeMechanic : LevelMechanicSO, IMechanicTextInfo, ILevelInstantiateUser
{
    [SerializeField] private GameObject _playerPrefab;
    public override bool ObjectiveCompleted => true;
    public GameObject UserPrefab => _playerPrefab;

    public string GetObjectiveText()
    {
        return "Find the exit";
    }
}
