using System.Collections.Generic;
using UnityEngine;

public class CoinTextBus : MonoBehaviour
{
    [SerializeField] private ObjectPool _objectPool;

    private List<Coin> coins = new List<Coin>();
    
    
    private void Awake()
    {
        FindAnyObjectByType<Spawner>().OnCoinSpawn+=RegisterCoin;
    }

    private void OnCoinFlipped(int value, Vector2 position)
    {
        if (!_objectPool.CanGetObject()) return;

        GameObject textObject = _objectPool.Get();
        if (textObject == null) return;
        
        textObject.transform.position = position;
        AmountText amountText = textObject.GetComponent<AmountText>();
        amountText.PlayAnim(value);
        amountText.OnComplete+=ReturnText;
    }

    public void ReturnText(AmountText amountText)
    {
        _objectPool.Return(amountText.gameObject);
        amountText.OnComplete-=ReturnText;
    }


    public void RegisterCoin(GameObject gameObject, Coin coin)
    {
        if (!coins.Contains(coin))
        {
            coins.Add(coin);
            coin.OnCoinFlipEnd+=OnCoinFlipped;
        }
    }

    private void UnregisterCoin(Coin coin)
    {
        if (coins.Contains(coin))
        {
            coins.Remove(coin);
            coin.OnCoinFlipEnd-=OnCoinFlipped;
        }
    }

    private void OnDestroy()
    {
        foreach (Coin coin in coins)
        {
            coin.OnCoinFlipEnd-=OnCoinFlipped;
        }
    }


}
