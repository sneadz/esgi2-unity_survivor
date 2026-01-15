using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerHealth
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player hit! HP: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player dead");
    }
}