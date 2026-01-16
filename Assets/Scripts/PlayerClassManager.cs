using UnityEngine;

/// <summary>
/// Gère la classe actuelle du joueur et applique ses stats
/// aux scripts de tir (AutoShoot, Gun, etc.).
/// </summary>
public class PlayerClassManager : MonoBehaviour
{
    [Header("Classe actuelle")]
    public PlayerClassData currentClass;

    [Header("Références tir")]
    public AutoShoot autoShoot; // optionnel

    void Start()
    {
        ApplyCurrentClass();
    }

    /// <summary>
    /// Applique les stats de la classe actuelle aux scripts concernés.
    /// </summary>
    public void ApplyCurrentClass()
    {
        if (currentClass == null) return;

        if (autoShoot != null)
        {
            autoShoot.fireRate = currentClass.autoFireRate;
            autoShoot.range = currentClass.autoRange;
            autoShoot.damage = currentClass.autoDamage;
            if (currentClass.autoBulletPrefab != null)
                autoShoot.bulletPrefab = currentClass.autoBulletPrefab;
        }
    }

    /// <summary>
    /// Permet de changer de classe par code (UI, input, etc.).
    /// </summary>
    public void SetClass(PlayerClassData newClass)
    {
        currentClass = newClass;
        ApplyCurrentClass();
    }
}

