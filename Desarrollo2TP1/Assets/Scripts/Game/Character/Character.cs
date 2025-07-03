using System;
using UnityEngine;

/// <summary>
/// Base class for all characters in the game.
/// </summary>
public abstract class Character : MonoBehaviour, IDamageable
{
    public float MaxHealth { get; set; }
    public float Damage { get; set; }
    public float CurrentHealth { get; protected set; }

    public bool IsWeaponUser { get; protected set; }

    //protected HealthBar _healthBar;
    protected IHealthBar _healthBar;
         
    virtual protected void Start()
    {
        MaxHealth = 100f;
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
        CurrentHealth -= damage;

        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            Die();
        }
        else
            _healthBar.UpdateHealth(CurrentHealth, MaxHealth);
    }

    virtual protected void FixedUpdate()
    {
        if (GameManager.paused)
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
