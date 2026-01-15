using UnityEngine;

/// <summary>
/// Caméra vue du dessus (ou 3/4 haut) qui suit le joueur
/// avec une vue dégagée sur les 4 directions.
/// 
/// À utiliser sur la caméra principale, en lui donnant comme target le joueur.
/// </summary>
public class TopDownCamera : MonoBehaviour
{
    [Header("Cible")]
    public Transform target;

    [Header("Position")]
    [Tooltip("Décalage par rapport au joueur (X/Z = décalage horizontal, Y = hauteur).")]
    public Vector3 offset = new Vector3(0f, 15f, -10f);

    [Tooltip("Vitesse de suivi de la position.")]
    public float followSpeed = 10f;

    [Header("Rotation")]
    [Tooltip("Angle vertical de la caméra (60-80 pour une bonne vue dégagée).")]
    [Range(30f, 85f)]
    public float pitch = 60f;

    [Tooltip("Angle horizontal fixe (0 = plein nord, 45 = diagonale, etc.).")]
    public float yaw = 0f;

    [Tooltip("Lis­sage de la rotation.")]
    public float rotationDamping = 10f;

    void LateUpdate()
    {
        if (!target) return;

        // 🔹 Position de la caméra (au-dessus / légèrement en diagonale)
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        // 🔹 Rotation fixe (vue du dessus dégagée)
        Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationDamping * Time.deltaTime
        );
    }
}

