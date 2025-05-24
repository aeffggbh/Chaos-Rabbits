using UnityEngine;
using UnityEngine.Rendering;

public abstract class Character : MonoBehaviour
{
    public bool IsWeaponUser { get; set; }
    public GameObject Obj { get; set; }

    public float damage;
    public float maxHealth;
    public float currentHealth;

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
    public abstract void Die();

    public void DeleteCharacterObject()
    {
        if (Obj != null)
        {
            Destroy(Obj);
            Obj = null;
        }
    }
}
