using UnityEngine;

/// <summary>
/// Données d'une classe de joueur (stats de tir, etc.).
/// Crée des instances via : clic droit dans le Project -> Create -> Player -> Class Data.
/// </summary>
[CreateAssetMenu(fileName = "NewPlayerClass", menuName = "Player/Class Data")]
public class PlayerClassData : ScriptableObject
{
    [Header("Infos")]
    public string className = "Default";
    [TextArea] public string description;

    [Header("Tir automatique")]
    public float autoFireRate = 1f;
    public float autoRange = 10f;
    public int autoDamage = 10;
    public Projectile autoBulletPrefab;
}

