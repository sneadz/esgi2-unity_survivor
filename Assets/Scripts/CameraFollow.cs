using UnityEngine;

public class CameraTPS : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Distance")]
    public float distance = 6f;
    public float height = 2f;
    public float followSpeed = 10f;

    [Header("Rotation")]
    public float sensitivity = 200f;
    public float minY = -30f;
    public float maxY = 60f;

    float yaw;
    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (!target) return;

        // 🔹 Souris
        yaw += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
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