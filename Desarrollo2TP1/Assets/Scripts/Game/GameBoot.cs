using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible for bootstrapping the game and initializing the GameManager.
/// </summary>
// This class is responsible for initializing the game and setting up the GameManager at the start of the game
public class GameBoot : MonoBehaviour
{
    private void Start()
    {
        //if (!GameManager.initialized)
        //    InitializeGameManager();
        //else
        //    Debug.Log("Not resetting game");
    }

    /// <summary>
    /// Initializes the game by setting up the GameManager
    /// </summary>
    private void InitializeGameManager()
    {
        //if (GameManager.savedPlayer != null)
        //    return;

        //Debug.Log("Resetting game");

        //GameManager.initialized = true;
    }



}
