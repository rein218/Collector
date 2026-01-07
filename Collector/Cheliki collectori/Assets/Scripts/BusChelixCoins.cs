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

    [SerializeField] List<Coin> coinsBronzeList = new List<Coin>();
    [SerializeField] List<Chelix> chelixList = new List<Chelix>();


    public Coin FindGoalForChelix()
    {
        var coinsBronzeListShuffled = coinsBronzeList.OrderBy(item => Guid.NewGuid());

        
        foreach (Coin coinBronze in coinsBronzeListShuffled)
        {
            if (coinBronze.isOccupied) continue;

            return coinBronze;
        }
        return null;
    }

    public void AddToCoinBronzeList(Coin newCoinBronze)
    {
        coinsBronzeList.Add(newCoinBronze);
    }
    public void AddToChelixList(Chelix newChelix)
    {
        chelixList.Add(newChelix);
    }

    public bool CoinBronzeListIsEmpty()
    {
        if (coinsBronzeList.Count > 0) return false;

        return true;
    }

    public void SetSpeedOfAllChelix(ItemData itemData)
    {
        float newSpeed = itemData.SpecialCurrentValue;
        foreach (Chelix chel in chelixList)
        {
            chel.SetNewSpeed(newSpeed);
        }
    }

    public void SetAllCoinsBronzeValue(ItemData itemData)
    {
        int newCoinValue = (int) itemData.SpecialCurrentValue;
        foreach (Coin coinBronze in coinsBronzeList)
        {
            coinBronze.SetNewCoinValue(newCoinValue);
        }
    }


}