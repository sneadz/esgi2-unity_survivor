using UnityEngine;

public class EnemyHealth : MonoBehaviour, IEnemyHealth
{
    [SerializeField] private EnemyData enemyData;
    public float maxHealth = 30;
    private float currentHealth;
    private Renderer meshRenderer;
    
    [Header("UI")]
    public WorldSpaceHealthBar healthBar;

    [Header("Drop XP")]
    [Tooltip("Prefab de l'orbe d'XP à faire tomber à la mort.")]
    public XPOrb xpOrbPrefab;
    [Tooltip("Montant d'XP donné par cet ennemi (surchage la valeur du prefab si > 0).")]
    public int xpOnDeath = 10;
    [Tooltip("Prefab d'orbe RARE (par ex. 50 XP). Si null, seule l'orbe normale est utilisée.")]
    public XPOrb rareXpOrbPrefab;
    [Range(0f, 1f)]
    [Tooltip("Chance qu'une orbe rare apparaisse à la place de l'orbe normale (0.1 = 10%).")]
    public float rareOrbChance = 0.1f;
    
    void Start()
    {
        // Initialisation : on prend les stats du ScriptableObject
        float baseMax = enemyData != null ? enemyData.maxHealth : maxHealth;
        maxHealth = baseMax * PlayerXP.enemyHealthMultiplier;
        currentHealth = maxHealth;
        meshRenderer = GetComponent<Renderer>();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(currentHealth);
        }
    }
    
    // Fonction appelée par Jules-Edouard quand son projectile touche l'ennemi
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " hit! HP: " + currentHealth);
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
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
        // Drop d'XP
        if (xpOrbPrefab != null || rareXpOrbPrefab != null)
        {
            // Choix entre orbe normale et orbe rare
            XPOrb prefabToUse = xpOrbPrefab;
            if (rareXpOrbPrefab != null && Random.value < rareOrbChance)
            {
                prefabToUse = rareXpOrbPrefab;
            }

            if (prefabToUse != null)
            {
                XPOrb orb = Instantiate(
                    prefabToUse,
                    transform.position,
                    Quaternion.identity
                );

                // Si on a configuré un montant d'XP spécifique pour cet ennemi, on l'applique
                if (xpOnDeath > 0)
                {
                    orb.xpAmount = xpOnDeath;
                }
            }
        }

        Debug.Log(gameObject.name + " dead");
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