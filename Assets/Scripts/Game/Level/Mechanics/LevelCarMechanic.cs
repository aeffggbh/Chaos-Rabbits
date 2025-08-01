
using UnityEngine;

[CreateAssetMenu(fileName = "CarMechanic", menuName = "ScriptableObjects/LevelMechanics/CarMechanic")]
public class LevelCarMechanic : LevelMechanicSO, IMechanicTextInfo, ILevelInstanceUser
{
    [SerializeField] private GameObject _carPrefab;
    public override bool ObjectiveCompleted => true;
    public GameObject UserPrefab => _carPrefab;

    public string GetObjectiveText()
    {
        return "Wow, you have a car? You should drive it to the end!";
    }
}