using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private bool cursorVisibleOnStart = true;

    void Awake()
    {
        ApplyCursorState(cursorVisibleOnStart);
    }

    void Update()
    {
        // Toggle avec Échap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorVisibleOnStart = !cursorVisibleOnStart;
            ApplyCursorState(cursorVisibleOnStart);
        }
    }

    void ApplyCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible 
            ? CursorLockMode.None 
            : CursorLockMode.Locked;
    }
}