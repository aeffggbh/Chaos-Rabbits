using TMPro;
using UnityEngine;

/// <summary>
/// A trigger that checks if the player has finished the level and transfers it to the next level.
/// </summary>
public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private IEnemyManager _enemyManager;
    [SerializeField] private int _enemyCounter;
    [SerializeField] private int _enemyTotal;
    [SerializeField] private TextMeshProUGUI _enemyCounterText;

    private void Start()
    {
        if (ServiceProvider.TryGetService<EnemyManager>(out var enemyManager))
            _enemyManager = enemyManager;

        if (_enemyManager != null)
        {
            _enemyCounter = 0;

            _enemyTotal = _enemyManager.Enemies.Count;
        }
        else
            Debug.LogWarning("EnemyManager not found");
    }

    private void Update()
    {
        if (CheatsController.Instance != null)
            if (CheatsController.Instance.levelTriggerLocation != transform)
                CheatsController.Instance.levelTriggerLocation = transform;

        if (!GameSceneController.Instance.IsSceneLoaded(GameplayScene.FinalLevelIndex))
        {
            if (_enemyTotal <= 0)
                _enemyTotal = _enemyManager.Enemies.Count;

            if (_enemyManager != null && _enemyCounterText)
            {
                _enemyCounter = _enemyTotal - _enemyManager.Enemies.Count;
                _enemyCounterText.SetText(_enemyCounter + " / " + _enemyTotal);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            OnTrigger();
    }

    /// <summary>
    /// Checks the current scene and transfers the player to the next level or the win scene if it's the final level.
    /// </summary>
    private void OnTrigger()
    {
        if (GameSceneController.Instance.IsSceneLoaded(GameplayScene.FinalLevelIndex))
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameWinState(), gameObject));
        else if (_enemyManager.Enemies.Count == 0)
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, true));
    }

}
