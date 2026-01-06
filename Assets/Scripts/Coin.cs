using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float RotationSpeed = 30;

    public float Amplitude = 0.2f;
    public float Frequency = 4;

    private float _yOffset;

    public static event Action OnCoinCollected;

    void Start()
    {
        _yOffset = transform.position.y;
    }
    
    void Update()
    {
        transform.eulerAngles += new Vector3(0, Time.deltaTime * RotationSpeed, 0);
        
        Vector3 pos = transform.position;
        pos.y = _yOffset + Amplitude * Mathf.Sin(Time.time * Frequency);
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}
