using UnityEngine;

/// <summary>
/// Utilitaires centralisés pour appliquer des dégâts aux différentes cibles.
/// Évite la duplication entre Projectile, Gun (hitscan) et AutoShoot (hitscan).
/// </summary>
public static class DamageUtility
{
    /// <summary>
    /// Applique des dégâts à partir d'un Collider (souvent reçu par un Raycast ou un trigger).
    /// Retourne true si des dégâts ont été appliqués à une cible valide (Player ou Enemy).
    /// </summary>
    public static bool ApplyDamage(Collider collider, int damage)
    {
        if (collider == null) return false;
        return ApplyDamage(collider.gameObject, damage);
    }

    /// <summary>
    /// Applique des dégâts à partir d'un Component.
    /// </summary>
    public static bool ApplyDamage(Component component, int damage)
    {
        if (component == null) return false;
        return ApplyDamage(component.gameObject, damage);
    }

    /// <summary>
    /// Applique des dégâts à partir d'un GameObject.
    /// </summary>
    public static bool ApplyDamage(GameObject go, int damage)
    {
        if (go == null) return false;

        // Priorité: Ennemis, puis Joueur.
        var enemy = go.GetComponentInParent<IEnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            return true;
        }

        var player = go.GetComponentInParent<IPlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
            return true;
        }

        return false;
    }
}
