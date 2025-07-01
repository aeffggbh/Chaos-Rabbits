using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxHealth(float maxHealth)
    {
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
        this.transform.forward = CinemachineManager.Instance.transform.forward;
    }
}
