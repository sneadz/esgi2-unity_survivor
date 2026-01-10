using UnityEngine;

public class EnemyHealth : MonoBehaviour, IEnemyHealth
{
    public int maxHealth = 30;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " hit! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " dead");
        Destroy(gameObject);
    }
}