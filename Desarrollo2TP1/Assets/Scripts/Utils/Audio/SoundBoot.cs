using UnityEngine;

public class SoundBoot : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;

    private void Start()
    {
        if (_soundManager)
            ServiceProvider.SetService<SoundManager>(_soundManager, true);
        else
            Debug.LogError(nameof(_soundManager) + " does not exist");
    }
}
