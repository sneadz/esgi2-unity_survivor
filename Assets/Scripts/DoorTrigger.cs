using System.Collections;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _door;

    [SerializeField] private float _openingDuration = 2;
    
    void OnTriggerEnter(Collider other)
    {
        // Animation porte
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        Vector3 posA = _door.transform.position;
        Vector3 posB = posA - new Vector3(0, 4, 0);

        float percent = 0;
        float time = 0;
        while (time < _openingDuration)
        {
            percent = time / _openingDuration;
            time += Time.deltaTime;
            _door.transform.position = Vector3.Lerp(posA, posB, percent);
            yield return null;
        }
    }
}
