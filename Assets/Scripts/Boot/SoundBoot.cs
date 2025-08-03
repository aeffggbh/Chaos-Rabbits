using UnityEngine;

/// <summary>
/// Boots up the sound
/// </summary>
public class SoundBoot : MonoBehaviour
{
    [SerializeField] private SoundContainer _soundContainer;

    private void Awake()
    {
        if (_soundContainer)
            ServiceProvider.SetService<SoundContainer>(_soundContainer, true);
        else
            Debug.LogError(nameof(_soundContainer) + " does not exist");
    }
}
