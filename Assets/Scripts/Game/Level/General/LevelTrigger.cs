using TMPro;
using UnityEngine;

/// <summary>
/// A trigger that checks if the player has finished the level and transfers it to the next level.
/// </summary>
public class LevelTrigger : MonoBehaviour
{
    ILevel level;

    private void Start()
    {
        level = GetComponent<ILevel>();
        if (level == null)
        {
            Debug.LogError("LevelTrigger: No ILevel component found on the GameObject.");
            return;
        }
    }

    private void Update()
    {
        if (CheatsController.Instance != null)
            if (CheatsController.Instance.levelTriggerLocation != transform)
                CheatsController.Instance.levelTriggerLocation = transform;
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
        level?.Trigger();
    }
}
