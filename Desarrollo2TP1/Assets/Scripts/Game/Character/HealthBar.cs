using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IHealthBar
{
    [SerializeField] private Slider slider;

    private float _maxHealth = 0f;

    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetCurrentHealth(float health)
    {
        slider.value = health;
    }

    private void FixedUpdate()
    {
        //copiar forward de la camara accediendo a ella con el service
        this.transform.forward = CineMachineManager.Instance.transform.forward;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (_maxHealth < maxHealth)
            SetMaxHealth(maxHealth);
        SetCurrentHealth(currentHealth);
    }
}
