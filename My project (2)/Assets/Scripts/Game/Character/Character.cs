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

    /// <summary>
    /// Makes the character take damage and updates its health.
    /// </summary>
    /// <param name="damage"></param>
    virtual public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
            currentHealth = 0f;
        }
        Debug.Log("Current health: " + currentHealth);
    }

    virtual protected void FixedUpdate()
    {
        if (GameManager.paused)
            return;
    }

    /// <summary>
    /// Handles the death of the character.
    /// </summary>
    public abstract void Die();

    /// <summary>
    /// Deletes the character object from the game.
    /// </summary>
    public void DeleteCharacterObject()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
