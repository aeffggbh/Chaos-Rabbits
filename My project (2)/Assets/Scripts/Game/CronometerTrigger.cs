using System;
using UnityEngine;

public class CronometerTrigger : MonoBehaviour
{
    [SerializeField] private float _goalTime;
    [SerializeField] private float _currentTime;
    private bool isCronoActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            if (!isCronoActive)
                isCronoActive = true;
    }

    private void Awake()
    {
        if (_goalTime < 0.1f)
        {
            Debug.LogWarning(nameof(_goalTime) + "is too small");
            _goalTime = 60f;
        }
    }

    private void Update()
    {
        if (isCronoActive)
            _currentTime += Time.deltaTime;

        if (_currentTime > _goalTime)
            //if it reaches this, it means it didn't arrive in time!
            SceneController.GoToScene(SceneController.Scenes.GAMEOVER);
    }
}