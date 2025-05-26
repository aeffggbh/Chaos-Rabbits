using TMPro;
using UnityEngine;

/// <summary>
/// A trigger that checks if the player has finished the level and transfers it to the next level.
/// </summary>
public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private int _enemyCounter;
    [SerializeField] private int _enemyTotal;
    [SerializeField] private TextMeshProUGUI _enemyCounterText;

    private void Start()
    {
        if (!_enemyManager)
        {
            if (ServiceProvider.TryGetService<EnemyManager>(out var enemyManager))
                _enemyManager = enemyManager;

            _enemyCounter = 0;
        }
        _enemyTotal = _enemyManager.enemies.Count;
    }

    private void Update()
    {
        if (SceneController.currentScene != SceneController.GameStates.FINAL_LEVEL)
        {
            _enemyCounter = _enemyTotal - _enemyManager.enemies.Count;
            _enemyCounterText.SetText(_enemyCounter + " / " + _enemyTotal);
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
        SceneController.CheckCurrentScene();

        if (SceneController.currentScene == SceneController.GameStates.FINAL_LEVEL)
            SceneController.GoToScene(SceneController.GameStates.GAMEWIN);
        else
            //check if the enemies are all dead.
            if (_enemyManager.enemies.Count == 0)
                SceneController.GoToScene(SceneController.currentScene + 1);
    }

}
