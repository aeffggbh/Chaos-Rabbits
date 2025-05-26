using UnityEngine;

/// <summary>
/// A trigger that checks if the player has finished the level and transfers it to the next level.
/// </summary>
public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;

    private void Awake()
    {
        if (!_enemyManager)
        {
            _enemyManager = EnemyManager.instance;
            if (!_enemyManager)
                Debug.LogError("EnemyManager not found in the scene.");
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
        {
            SceneController.GoToScene(SceneController.GameStates.GAMEWIN);
        }
        else
        {
            //check if the enemies are all dead.
            if (_enemyManager.enemies.Count == 0)
                SceneController.GoToScene(SceneController.currentScene + 1);
        }
    }

}
