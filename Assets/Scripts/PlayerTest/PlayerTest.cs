using UnityEngine;

// Script de test temporaire pour vérifier les dégâts ennemis
public class PlayerTest : MonoBehaviour, IPlayerHealth
{
    public float maxHealth = 100f;
    private float currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Player démarre avec {currentHealth} HP");
    }
    
    // Fonction appelée par les ennemis quand ils attaquent
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        // AFFICHE dans la Console pour VOIR les dégâts
        Debug.Log($"Player a pris {damage} dégâts ! HP restants : {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("PLAYER EST MORT !");
        // Pour l'instant on ne fait rien, juste le log
    }
}