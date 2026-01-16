using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère l'XP du joueur et met à jour une barre d'XP sur le HUD.
/// </summary>
public class PlayerXP : MonoBehaviour
{
    [Header("Multiplicateurs globaux")]
    public static float enemyHealthMultiplier = 1f;
    public static float playerDamageMultiplier = 1f;
    public static float playerFireRateMultiplier = 1f;

    [Header("XP")]
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int level = 1;

    [Header("UI HUD")]
    public Slider xpSlider;

    void Start()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }
    }

    /// <summary>
    /// Ajoute de l'XP au joueur.
    /// À appeler quand un ennemi meurt, un objectif est complété, etc.
    /// </summary>
    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;

        // Gestion des niveaux multiples si on gagne beaucoup d'XP d'un coup
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }
    }

    void LevelUp()
    {
        level++;
        // Simple progression: chaque niveau demande un peu plus d'XP
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);
        
        // Scaling de la difficulté / puissance
        enemyHealthMultiplier *= 1.5f;    // les prochains ennemis ont +50% de vie
        playerDamageMultiplier *= 1.2f;   // le joueur fait +20% de dégâts
        playerFireRateMultiplier *= 1.1f; // le joueur tire 10% plus vite

        Debug.Log($"Player leveled up! Level: {level}, Next XP: {xpToNextLevel}, Enemy HP x{enemyHealthMultiplier:F2}, Player Dmg x{playerDamageMultiplier:F2}, FireRate x{playerFireRateMultiplier:F2}");
    }
}

