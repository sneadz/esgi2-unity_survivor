using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public Projectile bulletPrefab;
    public Transform muzzle;

    public float fireRate = 1f;   // tirs par seconde
    public float range = 10f;     // portée

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / fireRate)
        {
            timer = 0f;
            ShootNearestEnemy();
        }
    }

    void ShootNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (d < minDist && d <= range)
            {
                minDist = d;
                nearest = enemy;
            }
        }

        if (nearest == null) return;

        Vector3 direction = (nearest.transform.position - muzzle.position).normalized;

        Projectile bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            Quaternion.LookRotation(direction)
        );

        bullet.Launch(direction);
    }
}