using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelObjectiveHandler))]
[RequireComponent(typeof(LevelTrigger))]
public class Level : MonoBehaviour, ILevel
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private Transform _levelSpawn;
    [SerializeField] private GameObject _userGO;
    [SerializeField] private CheatsController _cheatsController;
    public List<LevelMechanicSO> Mechanics { get => _levelData.Mechanics; set => _levelData.Mechanics = value; }
    public int LevelIndex => _levelData.LevelIndex;
    public GameObject UserGO => _userGO;

    private void Awake()
    {
        if (_levelData == null)
            Debug.LogError("No level data");

        foreach (var mechanic in Mechanics)
            (mechanic as IInitMechanic)?.Init();

        EventProvider.Subscribe<IDeleteUserEvent>(OnDeleteUser);

        GameObject userObj = null;

        foreach (var mechanic in Mechanics)
            userObj = (mechanic as ILevelInstanceUser)?.UserPrefab;

        _userGO = GameObject.Instantiate(userObj);

        _userGO.transform.position = _levelSpawn.position;
        
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IDeleteUserEvent>(OnDeleteUser);
    }

    private void Start()
    {
        EventTriggerManager.Trigger<IUserSpawnedEvent>(new UserSpawnedEvent(_userGO, _userGO.transform));

        if (LevelIndex != GameplaySceneData.Level1Index)
            EventTriggerManager.Trigger<ILevelUpSoundEvent>(new LevelUpSoundEvent(gameObject));
    }

    private void OnDeleteUser(IDeleteUserEvent levelEvent)
    {
        if (UserGO)
            Destroy(UserGO);
    }

    private void Update()
    {
        foreach (var mechanic in Mechanics)
            (mechanic as IUpdateMechanic)?.Update();
    }

    public void Trigger()
    {
        if (LevelIndex == GameplaySceneData.FinalLevelIndex)
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameWinState(), gameObject));
        else if (AllObjectivesCompleted())
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, true));
        else
            EventTriggerManager.Trigger<ILogMessageEvent>(new LogMessageEvent(gameObject, "PENDING OBJECTIVES"));
    }

    public bool AllObjectivesCompleted()
    {
        foreach (var mechanic in Mechanics)
            if (!mechanic.ObjectiveCompleted)
                return false;

        return true;
    }
}
