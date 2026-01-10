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
        if (coinsXListsByType.TryGetValue(coinType, out var current))
        {
            // Make sure the list isn't null
            if (current.coinList == null)
            {
                Debug.LogError($"Coin list for {coinType} is null!");
                current.coinList = new List<Coin>();
            }
            coinsXListsByType[coinType] = (current.coinList, true);
            Debug.Log($"Unlocked {coinType}, now has {current.coinList.Count} coins");
        }
        else
        {
            Debug.LogError($"Coin type {coinType} not found in dictionary!");
        }
    }


    public Coin FindGoalForChelix()
    {
        List<Coin> coinsListShuffled = new List<Coin>();
        
        foreach (var kvp in coinsXListsByType)
        {
            if (!kvp.Value.isAvailable) continue;
            
            coinsListShuffled.AddRange(kvp.Value.coinList);
        }
        var random = new System.Random();
        coinsListShuffled = coinsListShuffled.OrderBy(x => random.Next()).ToList();
        
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