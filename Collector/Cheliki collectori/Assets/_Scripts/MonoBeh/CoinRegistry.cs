using System.Collections.Generic;
using UnityEngine;

public class CoinRegistry : MonoBehaviour
{
    public static CoinRegistry Instance { get; private set; }
    private List<Coin> _allCoins = new List<Coin>();

    private void Awake()
    {   
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(Coin coin)
    {
        if (coin == null) return;
        if (!_allCoins.Contains(coin))
        {
            _allCoins.Add(coin);
            EventBus.OnAvailableCoinsChanged?.Invoke();
        }
    }

    /// <summary>
    /// Получить любую свободную монету, которую помощник может перевернуть.
    /// </summary>
    public Coin GetAvailableCoinForHelper()
    {
        bool silverUnlocked = IsSilverUnlocked();
        bool goldUnlocked = IsGoldUnlocked();

        foreach (var coin in _allCoins)
        {
            if (!coin.isOccupied) continue;

            if (IsCoinAllowed(coin, silverUnlocked, goldUnlocked))
            {
                return coin;
            }
        }

        return null;
    }

    /// <summary>
    /// Получить случайную свободную монету для помощника.
    /// </summary>
    public Coin GetRandomAvailableCoinForHelper()
    {
        bool silverUnlocked = IsSilverUnlocked();
        bool goldUnlocked = IsGoldUnlocked();

        var candidates = new List<Coin>();
        foreach (var coin in _allCoins)
        {
            if (!coin.isOccupied && IsCoinAllowed(coin, silverUnlocked, goldUnlocked))
            {
                candidates.Add(coin);
            }
        }
        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }

    /// <summary>
    /// Получить ближайшую к указанной позиции свободную монету.
    /// </summary>
    public Coin GetClosestAvailableCoinForHelper(Vector3 position)
    {
        bool silverUnlocked = IsSilverUnlocked();
        bool goldUnlocked = IsGoldUnlocked();

        Coin closest = null;
        float minDist = Mathf.Infinity;

        foreach (var coin in _allCoins)
        {
            if (coin.isOccupied) continue;
            if (!IsCoinAllowed(coin, silverUnlocked, goldUnlocked)) continue;

            float dist = Vector3.Distance(position, coin.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = coin;
            }
        }

        return closest;
    }


    public int GetCoinCount(string id)
    {
        int count = 0;
        foreach (var coin in _allCoins)
        {
            if (coin.id == id) count++;
        }
        return count;
    }

    private bool IsCoinAllowed(Coin coin, bool silverUnlocked, bool goldUnlocked)
    {
        string id = coin.id;

        if (id == "coin_bronse") return true;
        else if (id == "coin_silver" && silverUnlocked) return true;
        else if (id == "coin_gold" && goldUnlocked) return true;

        return false;
    }
    private bool IsSilverUnlocked() => GameManager.Instance.IsFeatureUnlocked("helper_silver");
    private bool IsGoldUnlocked() => GameManager.Instance.IsFeatureUnlocked("helper_gold");
}