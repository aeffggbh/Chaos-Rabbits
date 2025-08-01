using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IHealthBar
{
    [SerializeField] private Image fillImage;

    private float _maxValue = 0f;

    private void Awake()
    {
        if (fillImage == null)
            Debug.LogError("No fill image");
    }

    public void SetMaxHealth(float maxHealth)
    {
        _maxValue = maxHealth;
        fillImage.fillAmount = maxHealth;
    }

    public void SetCurrentHealth(float health)
    {
        fillImage.fillAmount = health / _maxValue;
    }

    private void FixedUpdate()
    {
        Vector3 fw = CineMachineManager.Instance.transform.forward;

        transform.forward = fw;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (_maxValue <= maxHealth)
            SetMaxHealth(maxHealth);
        SetCurrentHealth(currentHealth);
    }
}
