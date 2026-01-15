// Script obsolète pour ce projet minimal (gestion joueur/ennemi/arme/tir uniquement)
#if false
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
#endif