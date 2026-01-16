using UnityEngine;

/// <summary>
/// Orbe d'XP ramassable par le joueur.
/// Ajoute de l'XP puis se détruit au contact.
/// </summary>
[RequireComponent(typeof(Collider))]
public class XPOrb : MonoBehaviour
{
    [Header("XP")]
    public int xpAmount = 10;

    [Header("Pickup")]
    [Tooltip("Tag utilisé pour identifier le joueur.")]
    public string playerTag = "Player";

    void Reset()
    {
        // S'assure que le collider est en mode trigger par défaut
        var col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Vérifie qu'on touche bien le joueur
        if (!other.CompareTag(playerTag)) return;

        // Récupère le composant PlayerXP sur le joueur (ou ses parents / enfants)
        PlayerXP xp = other.GetComponent<PlayerXP>();
        if (xp == null)
        {
            xp = other.GetComponentInParent<PlayerXP>();
        }
        if (xp == null)
        {
            xp = other.GetComponentInChildren<PlayerXP>();
        }

        if (xp != null)
        {
            xp.AddXP(xpAmount);
        }

        Destroy(gameObject);
    }
}

