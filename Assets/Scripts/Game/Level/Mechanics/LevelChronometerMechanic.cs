using UnityEngine;

[CreateAssetMenu(fileName = "ChronometerMechanic", menuName = "ScriptableObjects/LevelMechanics/ChronometerMechanic")]
public class LevelChronometerMechanic : LevelMechanicSO, IMechanicTextInfo, IUpdateMechanic, IMechanicInit, IMechanicInstantiateUser
{
    [SerializeField] private float _goalTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private GameObject _playerPrefab;
    private bool isChronoActive = false;
    private string _objectiveText;
    public override bool ObjectiveCompleted => _currentTime > 0;
    public GameObject UserPrefab => _playerPrefab;

    public void Init()
    {
        if (_goalTime < 0.1f)
        {
            Debug.LogWarning(nameof(_goalTime) + "is too small");
            _goalTime = 30f;
        }

        _currentTime = _goalTime;

        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent(null, "RUN!!!!!!"));
        EventProvider.Subscribe<IChronometerTriggerEvent>(OnTrigger);
    }

    private void OnTrigger(IChronometerTriggerEvent triggerEvent)
    {
        if (triggerEvent.Other.CompareTag("Player"))
            if (!isChronoActive)
                isChronoActive = true;
    }

    public void Update()
    {
        if (isChronoActive)
        {
            _currentTime -= Time.deltaTime;
            UpdateText();
        }

        if (DidNotArriveOnTime())
        {
            _currentTime = 0;
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameOverState(), null, true));
        }
    }

    /// <summary>
    /// Updates the text component with the current time in seconds.
    /// </summary>
    private void UpdateText()
    {
        _objectiveText = $"REACH THE EXIT BEFORE THE TIME RUNS OUT." +
            $" Time left: 00:{_currentTime:00}";
    }

    private bool DidNotArriveOnTime()
    {
        return _currentTime <= 0f;
    }

    public string GetObjectiveText()
    {
        return _objectiveText;
    }
}
