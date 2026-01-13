using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _coinSpawnDelay = 2f;

    private float _spawnDelay;

    void Update()
    {
        _spawnDelay -= Time.deltaTime;
        if (_spawnDelay <= 0)
        {
            Vector2 randomCircle = 20 * Random.insideUnitCircle;
            Vector3 randomPos = new Vector3(randomCircle.x, 1f, randomCircle.y);
            Instantiate(_coinPrefab, randomPos, _coinPrefab.transform.rotation);
            _spawnDelay = _coinSpawnDelay;
        }
    }
}
