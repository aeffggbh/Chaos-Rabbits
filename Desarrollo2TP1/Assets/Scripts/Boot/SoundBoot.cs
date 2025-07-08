using UnityEngine;

/// <summary>
/// Boots up the sound
/// </summary>
public class SoundBoot : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;

    private void Awake()
    {
        if (_soundManager)
            ServiceProvider.SetService<SoundManager>(_soundManager, true);
        else
            Debug.LogError(nameof(_soundManager) + " does not exist");
    }
}
