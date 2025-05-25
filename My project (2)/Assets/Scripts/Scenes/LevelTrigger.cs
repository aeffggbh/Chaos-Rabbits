using System;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;

    private void Awake()
    {
        if (!_enemyManager)
        {
            _enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            if (!_enemyManager)
                Debug.LogError("EnemyManager not found in the scene.");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger();
    }

    private void OnTrigger()
    {
        if (SceneController.currentScene == SceneController.Scenes.FINAL_LEVEL)
        {
            //check if the player arrived in time. It's cronometer based, to add difficulty.
        }
        else
        {
            //check if the enemies are all dead.
            if (_enemyManager.enemies.Count == 0)
            {
                SceneController.GoToScene(SceneController.currentScene + 1);
            }
        }
    }
}
