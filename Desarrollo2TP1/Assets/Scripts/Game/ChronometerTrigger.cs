using TMPro;
using UnityEngine;

/// <summary>
/// A trigger that starts a timer when the player enters it.
/// </summary>
public class ChronometerTrigger : MonoBehaviour
{
    [SerializeField] private float _goalTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private TextMeshProUGUI _text;
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

        _currentTime = _goalTime;

        EventTriggerManager.Trigger<ILogMessageEvent>(new LogMessageEvent(gameObject, "RUN!!!!!!"));
    }

    private void Update()
    {
        if (isCronoActive)
        {
            _currentTime -= Time.deltaTime;
            UpdateText();
        }

        if (_currentTime <= 0f)
            //if it reaches this, it means it didn't arrive in time!
            EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new GameOverState(), gameObject));
    }

    /// <summary>
    /// Updates the text component with the current time in seconds.
    /// </summary>
    private void UpdateText()
    {
        if (_text != null)
            _text.SetText($"Time left: 00:{_currentTime:00}");
        else
            Debug.LogWarning("TextMeshPro component is not assigned.");
    }
}