using System.Collections.Generic;

public interface ILevelData
{
    List<LevelMechanicSO> Mechanics { get; set; }
    int LevelIndex { get; }
}
