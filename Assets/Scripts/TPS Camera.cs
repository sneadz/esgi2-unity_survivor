using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 200f;
    public float minY = -30f;
    public float maxY = 60f;
    public float followSpeed = 10f;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 1️⃣ Suivre la position du joueur
        Vector3 targetPosition = player.position + Vector3.up * 1.5f;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        // 2️⃣ Lire la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 3️⃣ Rotation verticale caméra (haut / bas)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 4️⃣ Rotation HORIZONTALE du personnage (gauche / droite)
        player.Rotate(Vector3.up * mouseX);
    }
}