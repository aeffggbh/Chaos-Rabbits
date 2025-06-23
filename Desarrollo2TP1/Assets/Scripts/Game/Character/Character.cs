using UnityEngine;

/// <summary>
/// Base class for all characters in the game.
/// </summary>
public abstract class Character : MonoBehaviour
{
    public bool IsWeaponUser { get; set; }
    public float damage;
    public float maxHealth;
    public float currentHealth;
    protected HealthBar _healthbar;

    /// <summary>
    /// Makes the character take damage and updates its health.
    /// </summary>
    /// <param name="damage"></param>
    virtual public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            if (this is ExplodingEnemy &&
                this.currentHealth <= 0f)
                Destroy(this.gameObject);
            Die();
        }
        Debug.Log("Current health: " + currentHealth);
        _healthbar.SetCurrentHealth(currentHealth);
    }

    virtual protected void Start()
    {
        maxHealth = 100.0f;
        currentHealth = maxHealth;

        _healthbar = GetComponentInChildren<HealthBar>();
        if (!_healthbar)
            Debug.LogError("HealthBar component is missing on " + gameObject.name);
        else
            _healthbar.SetMaxHealth(maxHealth);
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
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
