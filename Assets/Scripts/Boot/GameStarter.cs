using UnityEngine;

/// <summary>
/// Starts the game
/// </summary>
public class GameStarter : MonoBehaviour
{
    /// <summary>
    /// It starts the game. All the previous boot operations happen on Awake()
    /// </summary>
    private void Start()
    {
        EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new MainMenuState(), gameObject));

        Destroy(gameObject);
    }
}
