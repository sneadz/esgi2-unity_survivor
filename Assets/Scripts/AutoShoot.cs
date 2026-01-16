using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public Projectile bulletPrefab;
    public Transform muzzle;
    [Header("Ciblage")]
    public Transform targetOverride; // Assignez ici l'entité précise à viser (instance de scène)
    public string targetTag = "Enemy"; // Tag utilisé pour la recherche si pas d'override
    [Tooltip("Couches physiques à considérer pour la recherche de cibles avec OverlapSphere")] 
    public LayerMask enemySearchMask = ~0; // par défaut: tout

    public float fireRate = 1f;   // tirs par seconde
    public float range = 10f;     // portée
    [Header("Dégâts")]
    public int damage = 10;       // dégâts appliqués en hitscan (et éventuellement pour équilibrage visuel)

    float timer;

    void Update()
    {
        // Sécurité: si fireRate <= 0, on ne tire pas
        float effectiveFireRate = fireRate * PlayerXP.playerFireRateMultiplier;
        if (effectiveFireRate <= 0f) return;

        timer += Time.deltaTime;

        if (timer >= 1f / effectiveFireRate)
        {
            timer = 0f;
            ShootNearestEnemy();
        }
    }

    void ShootNearestEnemy()
    {
        // Assure un muzzle minimal si manquant
        if (muzzle == null)
        {
            GameObject m = new GameObject("MuzzleAuto");
            m.transform.SetParent(transform);
            m.transform.localPosition = Vector3.forward * 0.5f;
            m.transform.localRotation = Quaternion.identity;
            muzzle = m.transform;
        }

        // 1) Si une cible explicite est fournie et valide dans la scène, on la vise
        Transform target = null;
        if (targetOverride != null && targetOverride.gameObject.scene.IsValid())
        {
            // Vérifie la distance
            float d = Vector3.Distance(transform.position, targetOverride.position);
            if (d <= range)
                target = targetOverride;
        }

        // 2) Sinon, on cherche l'ennemi le plus proche via la physique (OverlapSphere) en détectant IEnemyHealth
        if (target == null)
        {
            Transform best = null;
            float bestDist = Mathf.Infinity;

            // Recherche physique: trouve tous les colliders dans la portée sur les layers autorisés
            var hits = Physics.OverlapSphere(transform.position, range, enemySearchMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hits.Length; i++)
            {
                var col = hits[i];
                if (col == null) continue;
                // Ignore les colliders du tireur
                if (col.transform.IsChildOf(transform)) continue;

                var enemyHealth = col.GetComponentInParent<IEnemyHealth>();
                if (enemyHealth == null) continue;

                Transform t = col.transform;
                float d = Vector3.Distance(transform.position, t.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = t;
                }
            }

            // Fallback: si rien trouvé par composant (ex: pas de colliders ou mauvais layers), tente par Tag
            if (best == null)
            {
                string tagToUse = string.IsNullOrEmpty(targetTag) ? "Enemy" : targetTag;
                GameObject[] enemies = SafeFindGameObjectsWithTag(tagToUse);
                float minDist = Mathf.Infinity;
                GameObject nearest = null;
                foreach (GameObject enemy in enemies)
                {
                    if (enemy == null) continue;
                    float d = Vector3.Distance(transform.position, enemy.transform.position);
                    if (d < minDist && d <= range)
                    {
                        minDist = d;
                        nearest = enemy;
                    }
                }
                if (nearest != null) best = nearest.transform;
            }

            if (best != null) target = best;
        }

        // Direction vers l'ennemi le plus proche SI trouvé dans la portée,
        // sinon tir dans la direction de la CAMÉRA (fallback demandé)
        Vector3 direction;
        if (target != null)
        {
            direction = (target.position - muzzle.position).normalized;
        }
        else
        {
            // Utiliser l'orientation de la caméra si disponible
            Vector3 camFwd = Vector3.zero;
            if (Camera.main != null)
            {
                camFwd = Camera.main.transform.forward;
                // Tir horizontal par cohérence avec Gun.cs
                camFwd.y = 0f;
            }

            // Fallback si pas de caméra ou vecteur nul
            if (camFwd == Vector3.zero)
            {
                camFwd = (muzzle != null ? muzzle.forward : transform.forward);
            }

            direction = camFwd.normalized;
        }

        if (bulletPrefab != null)
        {
            Projectile bullet = Instantiate(
                bulletPrefab,
                muzzle.position,
                Quaternion.LookRotation(direction)
            );

            // Passer le tireur comme owner pour ignorer les collisions avec lui
            bullet.Launch(direction, gameObject);
        }
        else
        {
            // Fallback hitscan si pas de projectile
            if (Physics.Raycast(muzzle.position, direction, out RaycastHit hit, range))
            {
                // Centralisé via DamageUtility, avec scaling de dégâts du joueur
                int finalDamage = Mathf.RoundToInt(damage * PlayerXP.playerDamageMultiplier);
                DamageUtility.ApplyDamage(hit.collider, finalDamage);
            }
            Debug.DrawRay(muzzle.position, direction * range, Color.cyan, 0.2f);
        }
    }

    // Certains projets lèvent une exception si le tag n'existe pas. Cette méthode évite de casser l'exécution.
    static GameObject[] SafeFindGameObjectsWithTag(string tag)
    {
        try
        {
            return GameObject.FindGameObjectsWithTag(tag);
        }
        catch
        {
            return System.Array.Empty<GameObject>();
        }
    }
}