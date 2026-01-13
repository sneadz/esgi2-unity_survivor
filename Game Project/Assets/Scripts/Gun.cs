using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile bulletPrefab;
    public Transform muzzle;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        // 1️⃣ Direction du regard
        Vector3 direction = Camera.main.transform.forward;

        // 2️⃣ On enlève la hauteur (tir horizontal)
        direction.y = 0f;

        // Sécurité
        if (direction == Vector3.zero)
            direction = transform.forward;

        direction.Normalize();

        // 3️⃣ Spawn + tir
        Projectile bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            Quaternion.LookRotation(direction)
        );

        bullet.Launch(direction);
    }
}