using UnityEngine;

public class TPSAim : MonoBehaviour
{
    public Transform muzzle;
    public float maxDistance = 100f;

    void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, maxDistance))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(maxDistance);

        // Rotation MONDIALE (pas locale)
        Vector3 direction = targetPoint - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}