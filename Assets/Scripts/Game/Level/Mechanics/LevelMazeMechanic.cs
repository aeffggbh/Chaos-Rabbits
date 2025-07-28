using UnityEngine;

[CreateAssetMenu(fileName = "MazeMechanic", menuName = "ScriptableObjects/LevelMechanics/MazeMechanic")]
public class LevelMazeMechanic : LevelMechanicSO, IMechanicTextInfo
{
    public override bool ObjectiveCompleted => true;

    public string GetObjectiveText()
    {
        return "Find the exit";
    }
}
