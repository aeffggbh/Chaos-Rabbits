using UnityEngine;

public abstract class LevelMechanicSO : ScriptableObject, ILevelMechanic
{
    public abstract bool ObjectiveCompleted { get; }
}