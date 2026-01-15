using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 10;

    Rigidbody rb;
    Collider _collider;
    GameObject _owner;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Launch(Vector3 direction)
    {
        // Backward compatibility: no owner specified
        Launch(direction, null);
    }

    // Nouvelle surcharge: permet de définir un owner pour ignorer les collisions avec le tireur
    public void Launch(Vector3 direction, GameObject owner)
    {
        _owner = owner;

        // Ignorer la collision avec le tireur (owner) pour éviter d'impacter le Player immédiatement
        if (_owner != null && _collider != null)
        {
            var ownerColliders = _owner.GetComponentsInChildren<Collider>();
            foreach (var oc in ownerColliders)
            {
                if (oc != null)
                    Physics.IgnoreCollision(_collider, oc, true);
            }
        }

        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Ne pas toucher l'owner (sécurité supplémentaire si les collisions ne sont pas ignorées par PhysX)
        if (_owner != null && other.transform.IsChildOf(_owner.transform))
        {
            return;
        }

        // Centralisation de l'application des dégâts
        if (DamageUtility.ApplyDamage(other, damage))
        {
            Destroy(gameObject);
        }
    }
}