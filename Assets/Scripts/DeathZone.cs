using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private GameObject _deathPopup;
    
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        _deathPopup.SetActive(true);
    }
}
