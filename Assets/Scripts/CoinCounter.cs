using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    private int _coins;

    void Start()
    {
        Coin.OnCoinCollected += Increment;
    }

    private void OnDestroy()
    {
        Coin.OnCoinCollected -= Increment;
    }

    public void Increment()
    {
        ++_coins;
        _text.text = $"Coins: {_coins}";
    }
}
