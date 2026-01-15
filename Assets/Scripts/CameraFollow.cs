using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTPS : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Distance")]
    public float distance = 6f;
    public float height = 2f;
    public float followSpeed = 10f;

    [Header("Rotation")]
    public float sensitivity = 5000f;
    public float minY = -30f;
    public float maxY = 60f;

    float yaw;
    float pitch;

    void Start()
    {
        // La caméra est fixe: on ne verrouille plus le curseur ni ne cache la souris
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Caméra FIXE: on n'utilise plus la souris pour modifier yaw/pitch
        // yaw et pitch restent ceux définis au Start (orientation de la caméra dans la scène)
        // On conserve le pitch actuel (initial) et on le borne tout de même au cas où il serait modifié ailleurs
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);

        // 🔹 Position FIXE derrière le joueur
        Vector3 desiredPosition =
            target.position
            - rot * Vector3.forward * distance
            + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        // 🔹 Rotation caméra
        transform.rotation = rot;
    }
}