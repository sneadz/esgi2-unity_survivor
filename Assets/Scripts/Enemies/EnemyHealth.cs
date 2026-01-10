using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    private float currentHealth;
    private Renderer meshRenderer;
    
    void Start()
    {
        // Initialisation : on prend les stats du ScriptableObject
        currentHealth = enemyData.maxHealth;
        meshRenderer = GetComponent<Renderer>();
    }
    
    // Fonction appelée par Jules-Edouard quand son projectile touche l'ennemi
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        // Feedback visuel pour montrer que l'ennemi est touché
        if (meshRenderer != null)
        {
            StartCoroutine(FlashRed());
        }
        
        // Vérification de la mort
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // TODO: Yanis ajoutera le drop d'XP ici
        Destroy(gameObject);
    }
    
    // Coroutine qui fait clignoter l'ennemi en rouge pendant 0.1s
    private System.Collections.IEnumerator FlashRed()
    {
        Color originalColor = meshRenderer.material.color;
        meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material.color = originalColor;
    }
}