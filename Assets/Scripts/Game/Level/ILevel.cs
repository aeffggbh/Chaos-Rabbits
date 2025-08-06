using UnityEngine;

public interface ILevel : ILevelData
{
    /// <summary>
    /// Saves a gameobject for the user that spawns in the level
    /// </summary>
    public GameObject UserGO {  get; }
    /// <summary>
    /// Tries to trigger the next level
    /// </summary>
    void TriggerNextLevel();
}
