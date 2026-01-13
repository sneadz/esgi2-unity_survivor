using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Survivor/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Statistiques")]
    public string enemyName = "Basic Enemy";
    public float maxHealth = 10f;
    public float moveSpeed = 2f;
    public float damage = 5f;
    public float attackCooldown = 1f;
    
    [Header("Visuel")]
    public Sprite enemySprite;
    public Color enemyColor = Color.red;
}