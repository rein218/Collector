using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class BusChelixCoins : MonoBehaviour
{
    public static BusChelixCoins Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        //load saved
        //instantiate saved
    }

    [SerializeField] List<Coin> coinsList = new List<Coin>();
    [SerializeField] List<Chelix> chelixList = new List<Chelix>();


    public Coin FindGoalForChelix()
    {
        var coinsListShuffled = coinsList.OrderBy(item => Guid.NewGuid());

        foreach (Coin coin in coinsListShuffled)
        {
            if (coin.IsOccupied) continue;

            return coin;
        }
        return null;
    }

    public void AddToCoinList(Coin newCoin)
    {
        coinsList.Add(newCoin);
    }
    public void AddToChelixList(Chelix newChelix)
    {
        chelixList.Add(newChelix);
    }


}