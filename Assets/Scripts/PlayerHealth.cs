using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerHealth
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player hit! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player dead");
        // Ici plus tard : Game Over, respawn, etc.
    }
}