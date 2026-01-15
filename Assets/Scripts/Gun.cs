using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile bulletPrefab;
    public Transform muzzle;
    [Header("Fallback (si pas de projectile)")]
    public float range = 50f;
    public int damage = 10;
    public LayerMask hitMask = ~0; // tout par défaut

    void Start()
    {
        // Si pas de muzzle assigné, on en crée un enfant au bout du canon
        if (muzzle == null)
        {
            GameObject m = new GameObject("MuzzleAuto");
            m.transform.SetParent(transform);
            m.transform.localPosition = Vector3.forward * 0.5f;
            m.transform.localRotation = Quaternion.identity;
            muzzle = m.transform;
        }
    }

    void Shoot()
    {
        // 1️⃣ Direction du regard
        Camera cam = Camera.main;
        Vector3 direction = cam != null ? cam.transform.forward : transform.forward;

        // 2️⃣ On enlève la hauteur (tir horizontal)
        direction.y = 0f;

        // Sécurité
        if (direction == Vector3.zero)
            direction = transform.forward;

        direction.Normalize();

        // 3️⃣ Spawn + tir projectile OU fallback hitscan si pas de prefab
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
            DoHitscan(muzzle.position, direction);
        }
    }

    void DoHitscan(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            // Centralisé via DamageUtility (Enemy ou Player selon le composant trouvé)
            DamageUtility.ApplyDamage(hit.collider, damage);
        }

        // Petit feedback visuel en mode Debug
        Debug.DrawRay(origin, direction * range, Color.yellow, 0.2f);
    }
}