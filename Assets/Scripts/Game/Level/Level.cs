using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main class for making levels
/// </summary>
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

        GameObject userObj = null;
        foreach (var mechanic in Mechanics)
        {
            (mechanic as IMechanicInit)?.Init();
            userObj = (mechanic as IMechanicInstantiateUser)?.UserPrefab;
        }

        if (userObj)
            _userGO = GameObject.Instantiate(userObj);

        foreach (var mechanic in Mechanics)
        {
            if (_userGO != null)
                (mechanic as IMechanicAddComponent)?.AddNeededComponent(_userGO);
        }

        _userGO.transform.position = _levelSpawn.position;

        EventProvider.Subscribe<IDeleteUserEvent>(OnDeleteUser);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IDeleteUserEvent>(OnDeleteUser);
    }

    private void Start()
    {
        EventTriggerer.Trigger<IUserSpawnedEvent>(new UserSpawnedEvent(_userGO, _userGO.transform));

        if (LevelIndex != GameplaySceneData.Level1Index)
            EventTriggerer.Trigger<ILevelUpSoundEvent>(new LevelUpSoundEvent(gameObject));
    }

    private void OnDeleteUser(IDeleteUserEvent levelEvent)
    {
        if (UserGO)
            Destroy(UserGO);
    }

    private void Update()
    {
        foreach (var mechanic in Mechanics)
            (mechanic as IMechanicUpdate)?.Update();
    }

    public void TriggerNextLevel()
    {
        if (LevelIndex == GameplaySceneData.FinalLevelIndex)
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameWinState(), gameObject, true));
        else if (AllObjectivesCompleted())
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, true));
        else
            EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(gameObject, "PENDING OBJECTIVES"));
    }

    public bool AllObjectivesCompleted()
    {
        foreach (var mechanic in Mechanics)
            if (!mechanic.ObjectiveCompleted)
                return false;

        return true;
    }
}
