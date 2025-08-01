using UnityEngine;

public interface ILevel : ILevelData
{
    public GameObject UserGO {  get; }
    void Trigger();
}
