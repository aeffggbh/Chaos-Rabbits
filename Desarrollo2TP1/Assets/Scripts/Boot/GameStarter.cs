using UnityEngine;

public class GameStarter : MonoBehaviour
{
    /// <summary>
    /// It starts the game. All the previous boot operations happen on Awake()
    /// </summary>
    void Start()
    {
        //actually load the menu here
        EventTriggerManager.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new MainMenuState(), gameObject));

        Destroy(gameObject);
    }
}
