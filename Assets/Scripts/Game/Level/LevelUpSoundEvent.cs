using UnityEngine;

public class LevelUpSoundEvent : ILevelUpSoundEvent
{
    private GameObject _triggeredByGO;
    public GameObject TriggeredByGO => _triggeredByGO;

    public LevelUpSoundEvent(GameObject TriggeredByGO)
    {
        _triggeredByGO = TriggeredByGO;
    }
}