using UnityEngine;

public class SkyBoxUpdater : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        RotateSkybox();
    }

    /// <summary>
    /// Rotates the skybox in settings
    /// </summary>
    private void RotateSkybox()
    {
        if (RenderSettings.skybox)
            RenderSettings.skybox.SetFloat("_Rotation", (Time.time * rotationSpeed) % 180);
    }
}
