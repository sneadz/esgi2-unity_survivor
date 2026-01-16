using UnityEngine;

/// <summary>
/// Fait apparaître des ennemis autour d'une cible (ex: le joueur).
/// La cadence et le nombre par vague augmentent progressivement dans le temps.
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Prefab de l'ennemi à instancier (obligatoire)")]
    public GameObject enemyPrefab;

    [Tooltip("Cible autour de laquelle spawn les ennemis. Si null, recherche par tag au Start.")]
    public Transform target;

    [Tooltip("Tag utilisé pour trouver la cible si 'target' est null au démarrage.")]
    public string targetTag = "Player";

    [Header("Tag des ennemis spawnés")]
    [Tooltip("Si non vide, applique ce Tag aux instances créées pour faciliter le ciblage.")]
    public string enemyTag = "Enemy";

    [Header("Rayon de spawn")]
    [Tooltip("Rayon max autour de la cible pour faire spawn les ennemis")]
    public float spawnRadius = 15f;

    [Tooltip("Rayon intérieur sans spawn autour de la cible (zone de sécurité)")]
    public float innerNoSpawnRadius = 3f;

    [Header("Progression de difficulté")]
    [Tooltip("Intervalle initial entre les vagues (en secondes)")]
    public float initialInterval = 3f;

    [Tooltip("Intervalle minimal atteint en fin de rampe")]
    public float minInterval = 0.75f;

    [Tooltip("Durée (en secondes) pour atteindre l'intervalle minimal et le max par vague")]
    public float rampDuration = 120f;

    [Tooltip("Nombre initial d'ennemis par vague")]
    public int initialPerWave = 1;

    [Tooltip("Nombre maximum d'ennemis par vague en fin de rampe")]
    public int maxPerWave = 6;

    [Header("Limite globale d'ennemis")]
    [Tooltip("Nombre maximum d'ennemis présents en même temps sur la carte (<= 0 = illimité).")]
    public int maxEnemies = 50;

    [Header("Placement au sol")]
    [Tooltip("Essayer d'aligner le spawn sur le sol via un raycast vers le bas")]
    public bool alignToGround = true;

    [Tooltip("Hauteur de départ du raycast pour chercher le sol")]
    public float groundRayStartHeight = 10f;

    [Tooltip("Décalage vertical au-dessus du sol pour éviter de spawn dans le mesh")] 
    public float groundOffset = 0.25f;

    [Tooltip("Masque de layer considéré comme 'sol'")]
    public LayerMask groundMask = ~0;

    float _timer;
    float _elapsed;

    void Start()
    {
        if (target == null && !string.IsNullOrEmpty(targetTag))
        {
            var go = GameObject.FindGameObjectWithTag(targetTag);
            if (go != null) target = go.transform;
        }
    }

    void Update()
    {
        if (enemyPrefab == null || target == null) return;

        _elapsed += Time.deltaTime;
        _timer += Time.deltaTime;

        float t = rampDuration > 0f ? Mathf.Clamp01(_elapsed / rampDuration) : 1f;
        float currentInterval = Mathf.Lerp(initialInterval, minInterval, t);
        currentInterval = Mathf.Max(0.05f, currentInterval); // garde-fou

        if (_timer >= currentInterval)
        {
            _timer = 0f;
            int count = Mathf.RoundToInt(Mathf.Lerp(initialPerWave, maxPerWave, t));
            count = Mathf.Max(1, count);
            SpawnWave(count);
        }
    }

    void SpawnWave(int count)
    {
        // Limite globale d'ennemis : si maxEnemies > 0, on ne spawn que si on n'a pas atteint la limite
        int allowedCount = count;
        if (maxEnemies > 0 && !string.IsNullOrEmpty(enemyTag))
        {
            int current = 0;
            try
            {
                current = GameObject.FindGameObjectsWithTag(enemyTag).Length;
            }
            catch
            {
                current = 0;
            }

            int remaining = maxEnemies - current;
            if (remaining <= 0)
            {
                return; // déjà au max, on ne spawn pas cette vague
            }

            allowedCount = Mathf.Min(count, remaining);
        }

        for (int i = 0; i < allowedCount; i++)
        {
            Vector3 pos = RandomPointOnAnnulus(target.position, innerNoSpawnRadius, spawnRadius);

            if (alignToGround)
            {
                // 1) Raycast vers le bas depuis au-dessus du point choisi
                Vector3 originDown = pos + Vector3.up * groundRayStartHeight;
                if (Physics.Raycast(originDown, Vector3.down, out RaycastHit hitDown, groundRayStartHeight * 2f, groundMask, QueryTriggerInteraction.Ignore))
                {
                    pos = hitDown.point + Vector3.up * groundOffset; // petit offset pour éviter d'être à l'intérieur du sol
                }
                else
                {
                    // 2) Fallback: si on n'a rien touché en descendant, essaye en montant depuis sous le point
                    Vector3 originUp = pos - Vector3.up * groundRayStartHeight;
                    if (Physics.Raycast(originUp, Vector3.up, out RaycastHit hitUp, groundRayStartHeight * 2f, groundMask, QueryTriggerInteraction.Ignore))
                    {
                        pos = hitUp.point + Vector3.up * groundOffset;
                    }
                    else
                    {
                        // 3) Dernier recours: décale légèrement au-dessus de la hauteur de la cible
                        pos.y = target.position.y + groundOffset;
                    }
                }
            }

            Quaternion rot = Quaternion.LookRotation((target.position - pos).normalized, Vector3.up);
            GameObject instance = Instantiate(enemyPrefab, pos, rot);

            // Optionnel: Assigner un tag à l'ennemi spawné pour que les systèmes par Tag le détectent
            if (!string.IsNullOrEmpty(enemyTag))
            {
                // Évite une exception si le tag n'existe pas dans le projet
                try { instance.tag = enemyTag; } catch { /* ignore */ }
            }
        }
    }

    static Vector3 RandomPointOnAnnulus(Vector3 center, float innerRadius, float outerRadius)
    {
        if (outerRadius < 0f) outerRadius = 0f;
        innerRadius = Mathf.Clamp(innerRadius, 0f, outerRadius);

        // Échantillonnage uniforme par aire sur un anneau
        float rInner2 = innerRadius * innerRadius;
        float rOuter2 = outerRadius * outerRadius;
        float u = Random.value;
        float r = Mathf.Sqrt(Mathf.Lerp(rInner2, rOuter2, u));
        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * r;
        return center + offset;
    }

    void OnDrawGizmosSelected()
    {
        Transform c = target != null ? target : transform;
        if (c == null) return;

        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.6f);
        DrawCircle(c.position, innerNoSpawnRadius);

        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.6f);
        DrawCircle(c.position, spawnRadius);
    }

    static void DrawCircle(Vector3 center, float radius, int segments = 40)
    {
        if (radius <= 0f) return;
        Vector3 prev = center + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float t = (i / (float)segments) * Mathf.PI * 2f;
            Vector3 next = center + new Vector3(Mathf.Cos(t) * radius, 0f, Mathf.Sin(t) * radius);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}
