using UnityEngine;

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

    private void OnTrigger()
    {
        SceneController.CheckCurrentScene();

        if (SceneController.currentScene == SceneController.Scenes.FINAL_LEVEL)
        {
            SceneController.GoToScene(SceneController.Scenes.GAMEWIN);
        }
        else
        {
            //check if the enemies are all dead.
            if (_enemyManager.enemies.Count == 0)
                SceneController.GoToScene(SceneController.currentScene + 1);
        }
    }
}
