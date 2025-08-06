using System.Collections.Generic;

/// <summary>
/// The data of a level
/// </summary>
public interface ILevelData
{
    /// <summary>
    /// Saves the mechanics of the level
    /// </summary>
    List<LevelMechanicSO> Mechanics { get; set; }
    /// <summary>
    /// Saves the scene index of the level
    /// </summary>
    int LevelIndex { get; }
}
