using UnityEngine;

/// <summary>
/// Base class for all characters in the game.
/// </summary>
public abstract class Character : MonoBehaviour, IDamageable, IHealthData
{
    public float MaxHealth { get; set; }
    public float Damage { get; set; }
    public float CurrentHealth { get; protected set; }

    protected IHealthBar _healthBar;

    virtual protected void Start()
    {
        MaxHealth = 100f;
        if (CurrentHealth <= 0.1f)
            CurrentHealth = MaxHealth;

        InitHealthBar();
    }

    private void InitHealthBar()
    {
        _healthBar = GetComponentInChildren<IHealthBar>();
        if (_healthBar != null)
            _healthBar.UpdateHealth(CurrentHealth, MaxHealth);
        else
            Debug.LogError($"{_healthBar} is null");
    }

    /// <summary>
    /// Makes the character take damage and updates its health.
    /// </summary>
    /// <param name="damage"></param>
    virtual public void TakeDamage(float damage)
    {
        SetHealth(CurrentHealth - damage, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            SetHealth(0, MaxHealth);
            Die();
        }
    }

    virtual protected void SetHealth(float health, float maxHealth)
    {
        CurrentHealth = health;

        if (_healthBar == null)
            _healthBar = GetComponentInChildren<IHealthBar>();

        if (health > 0f)
            _healthBar?.UpdateHealth(CurrentHealth, maxHealth);
    }

    virtual protected void FixedUpdate()
    {
        if (PauseManager.Paused)
            return;
    }

    /// <summary>
    /// Handles the death of the character.
    /// </summary>
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
