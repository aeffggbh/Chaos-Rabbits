using UnityEngine;

/// <summary>
/// Base scriptable object for level mechanics
/// </summary>
public abstract class LevelMechanicSO : ScriptableObject, ILevelMechanic
{
    public abstract bool ObjectiveCompleted { get; }
}