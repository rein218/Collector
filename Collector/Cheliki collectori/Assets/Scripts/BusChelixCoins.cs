using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class BusChelixCoins : MonoBehaviour
{
    public static BusChelixCoins Instance { get; private set; }

    [SerializeField] List<Chelix> chelixList = new List<Chelix>();
    [SerializeField]
    List<Coin> coinsBronzeList = new List<Coin>(),
                                coinsSilverList = new List<Coin>(),
                                coinsGoldList = new List<Coin>();

    Dictionary<ItemName, (List<Coin> coinList, bool isAvailable)> coinsXListsByType;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;


        coinsXListsByType = new Dictionary<ItemName, (List<Coin> coinList, bool isAvailable)>
        {
            { ItemName.NewCoinBronze, (coinsBronzeList, true) },
            { ItemName.NewCoinSilver, (coinsSilverList, false) },
            { ItemName.NewCoinGold, (coinsGoldList, false) }
        };


        //load saved
        //instantiate saved
    }



    public void AddToCoinsXList(Coin newCoinX, ItemName coinType)
    {
        coinsXListsByType[coinType].coinList.Add(newCoinX);
    }

    public bool CoinsAvailableListsIsEmpty()
    {
        foreach ((List<Coin> coinList, bool isAvailable) coinListsAndBools
            in coinsXListsByType.Values)
        {
            if (!coinListsAndBools.isAvailable) continue;

            if (coinListsAndBools.coinList.Count > 0) return false;
        }

        return true;
    }

    public void SetAllCoinsXValue(ItemData itemData)
    {
        int newCoinValue = (int)itemData.SpecialCurrentValue;
        foreach (Coin coinX in coinsXListsByType[itemData.ItemName].coinList)
        {
            coinX.SetNewCoinValue(newCoinValue);
        }
    }

    public void UnlockTypeOfCoinForChelix(ItemName coinType)
    {
        var current = coinsXListsByType[coinType];
        coinsXListsByType[coinType] = (current.coinList, true);
    }


public Coin FindGoalForChelix()
    {
        List<Coin> coinsListShuffled = new List<Coin>();
        foreach ((List<Coin> coinList, bool isAvailable) coinListsAndBools
            in coinsXListsByType.Values)
        {
            if (!coinListsAndBools.isAvailable) continue;

            coinsListShuffled.AddRange(coinListsAndBools.coinList.OrderBy(item => Guid.NewGuid()));
        }
        
        foreach (Coin coin in coinsListShuffled)
        {
            if (coin.isOccupied) continue;

            return coin;
        }
        return null;
    }

    public void AddToChelixList(Chelix newChelix)
    {
        chelixList.Add(newChelix);
    }

    public void SetSpeedOfAllChelix(ItemData itemData)
    {
        float newSpeed = itemData.SpecialCurrentValue;
        foreach (Chelix chel in chelixList)
        {
            chel.SetNewSpeed(newSpeed);
        }
    }
}