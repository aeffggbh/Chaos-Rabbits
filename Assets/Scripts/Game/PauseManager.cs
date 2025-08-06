using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the pause menu
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    private static bool _paused = false;

    public static bool Paused { get => _paused; set => _paused = value; }

    private void Start()
    {
        Time.timeScale = 1f;

        EventProvider.Subscribe<IPauseEvent>(Pause);
    }

    private void OnEnable()
    {
        pauseAction.action.started += OnPause;
    }

    private void OnDisable()
    {
        pauseAction.action.started -= OnPause;
    }

    private void OnDestroy()
    {
        pauseAction.action.started -= OnPause;
        EventProvider.Unsubscribe<IPauseEvent>(Pause);
    }

    /// <summary>
    /// Handles the pause action when the pause button is pressed.
    /// </summary>
    /// <param name="context"></param>
    private void OnPause(InputAction.CallbackContext context)
    {
        EventTriggerer.Trigger<IPauseEvent>(new PauseEvent(gameObject));
    }

    /// <summary>
    /// Toggles the paused state
    /// </summary>
    /// <param name="pauseEvent"></param>
    private void Pause(IPauseEvent pauseEvent)
    {
        Paused = !Paused;

        Time.timeScale = Paused ? 0f : 1f;

        if (Paused)
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new PauseMenuState(), gameObject));
        else
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateGameplayEvent(gameObject, false, false));
    }
}
