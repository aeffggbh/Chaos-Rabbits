using UnityEngine;

/// <summary>
/// Rotates the skybox from the render settings
/// </summary>
public class SkyBoxRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }

    private void Update()
    {
        RotateSkybox();
    }

    //TODO: Adjust the rotation so it actually rotates the full turn and not half a turn
    /// <summary>
    /// Rotates the skybox in settings
    /// </summary>
    private void RotateSkybox()
    {
        if (rotationSpeed <= 0)
            return;

        if (RenderSettings.skybox)
            RenderSettings.skybox.SetFloat("_Rotation", (Time.time * rotationSpeed) % 360);
    }
}
