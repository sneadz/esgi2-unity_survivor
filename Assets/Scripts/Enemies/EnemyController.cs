using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    
    private Transform player;
    private Rigidbody rb;
    private float attackTimer = 0f;
    
    void Start()
    {
        // Trouve le joueur dans la scène (Jules-Edouard doit mettre le tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        rb = GetComponent<Rigidbody>();
        
        // Configure le visuel de l'ennemi (couleur rouge par défaut)
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = enemyData.enemyColor;
        }
    }
    
    void Update()
    {
        // Si on n'a pas trouvé le joueur, on ne fait rien
        if (player == null) return;
        
        // Déplacement continu vers le joueur
        MoveTowardsPlayer();
        
        // Décompte du timer d'attaque
        attackTimer -= Time.deltaTime;
    }
    
    // Logique de déplacement : l'ennemi se dirige vers le joueur sur le plan horizontal (Y=0)
    private void MoveTowardsPlayer()
    {
        // Calcul de la direction, mais en gardant l'ennemi au sol (pas de mouvement en hauteur)
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // On ignore la hauteur
        direction = direction.normalized;
        
        // Application de la vitesse
        Vector3 movement = direction * enemyData.moveSpeed;
        movement.y = rb.linearVelocity.y; // On garde la gravité
        rb.linearVelocity = movement;
        
        // L'ennemi regarde vers le joueur
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    // Collision continue avec le joueur = attaque répétée
    private void OnCollisionStay(Collision collision)
    {
        // Vérifie si on touche le joueur ET si le cooldown d'attaque est terminé
        if (collision.gameObject.CompareTag("Player") && attackTimer <= 0f)
        {
            // Cherche le script de vie du joueur (interface que Jules-Edouard doit implémenter)
            IPlayerHealth playerHealth = collision.gameObject.GetComponent<IPlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyData.damage);
                attackTimer = enemyData.attackCooldown; // Reset du cooldown
            }
        }
    }
}