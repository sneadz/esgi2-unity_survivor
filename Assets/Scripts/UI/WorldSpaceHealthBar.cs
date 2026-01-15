using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Barre de vie en world-space à placer en enfant du perso / ennemi.
/// Se contente de faire face à la caméra, sans changer la scale.
/// </summary>
public class WorldSpaceHealthBar : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;

    [Header("Comportement")]
    public bool faceCamera = true;

    void LateUpdate()
    {
        // Caméra valide
        Camera cam = Camera.main;
        if (cam == null) cam = Camera.current;
        if (cam == null) return;

        // Orientation : même Y que la caméra, pas de tilt
        if (faceCamera)
        {
            Vector3 camEuler = cam.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, camEuler.y, 0f);
        }
    }

    public void SetMaxHealth(float max)
    {
        if (slider == null) return;
        slider.maxValue = max;
        slider.value = max;
    }

    public void SetHealth(float value)
    {
        if (slider == null) return;
        slider.value = value;
    }
}

