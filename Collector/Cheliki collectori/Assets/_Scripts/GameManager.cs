using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameConfig _gameConfig;
    private GameState _gameState;


    //cache  
    private Dictionary<string, int> _cachedCoinIncome = new();
    private Dictionary<string, float> _cachedCoinMoveDur = new();
    private Dictionary<string, float> _cachedCoinDoubleFlipChance = new();
    private float _cachedChelSpeedMoveDur;
    public void ClearCache()
    {
        _cachedCoinIncome.Clear();
        _cachedCoinMoveDur.Clear();
        _cachedCoinDoubleFlipChance.Clear();
        _cachedChelSpeedMoveDur = 0;
    }


    private void Awake()
    {
        if(Instance == null)
        Instance = this; 

        _gameState = new GameState();
        InitializeNewGameState();
    }

    private void InitializeNewGameState()
    {
        foreach (var itemConfig in _gameConfig.GetAllItems())
        {
            var itemState = new ItemState
            {
                id = itemConfig.Id,
                upgradeLevel = 0,
            };
            _gameState.AllItems.Add(itemState);
        }
    }

    public void Start()
    {
        EventBus.changeDollarsCountEvent?.Invoke(_gameState.dollarsCount);
        EventBus.changeAllTimeDollarsCountEvent?.Invoke(_gameState.allTimeDollarsCount);
        EventBus.changeFailsCountEvent?.Invoke(_gameState.failsCount);
        EventBus.OnStateChanged?.Invoke();
    }

    internal bool IsFeatureUnlocked(string id)
    {
        foreach (var itemConfig in _gameConfig.GetAllItems())
        {
            if(itemConfig.Id == id)
            {
                if ( _gameState.GetItemLevel(id)>0)
                return true;
                else return false;
            }
        }
        return false;
    }

    public int GetItemLevel(string itemId) => _gameState.GetItemLevel(itemId);

    public bool IsEnough(int dollarToSpend)
    {
        if (_gameState.dollarsCount >= dollarToSpend)
        {
            return true;
        }
        return false;
    }
    
    public bool TryToAddDollars(int dollarsToAdd)
    {
        if (dollarsToAdd <= 0)
        {
            return false;
        }

        _gameState.dollarsCount += dollarsToAdd;
        _gameState.allTimeDollarsCount+= dollarsToAdd;

        EventBus.changeDollarsCountEvent?.Invoke(_gameState.dollarsCount);
        EventBus.changeAllTimeDollarsCountEvent?.Invoke(_gameState.allTimeDollarsCount);
        return true;
    }

    public bool TryToSpendDollars(int dollarToSpend)
    {
        if (_gameState.dollarsCount - dollarToSpend >= 0)
        {
            _gameState.dollarsCount -= dollarToSpend;
            EventBus.changeDollarsCountEvent?.Invoke(_gameState.dollarsCount);
            return true;
        }
        return false;
    }

    public int GetCurrLevel(string id)
    {
        var item = _gameState.GetItemState(id);
        return item.upgradeLevel;
    }

    public bool CheckRequirements(string id) => CheckRequirements(_gameConfig.GetItemConfig(id).UnlockRequirements);

    public bool CheckRequirements(List<UnlockRequirement> requirements)
    {
        {
            if (requirements == null || requirements.Count == 0)
                return true; 

            return requirements.All(r => r.IsMet(_gameState));
        }
    }

    public float GetActualPrice(string id)
    {
        ItemState state = _gameState.GetItemState(id);
        var itemConfig = _gameConfig.GetItemConfig(id);
        float cost = 0;
        if(state.upgradeLevel == 0) cost = itemConfig.BaseCost;
        else                        cost = itemConfig.BaseCost +(itemConfig.CostAdd * Mathf.Pow(itemConfig.CostAddMultiplier, state.upgradeLevel));
        return cost;
    }

    public bool LevelUpItem(string id)
    {
        ShopItemConfig config = _gameConfig.GetItemConfig(id);
        if (config == null) return false;

        ItemState state = _gameState.GetItemState(id);
        if (state == null) return false; 

        if (state.upgradeLevel >= config.MaxLevel)
            return false;

        if (!CheckRequirements(config.UnlockRequirements))
            return false;

        float price = GetActualPrice(id);

        if (!TryToSpendDollars((int)price))
            return false;

        state.upgradeLevel++;
        

        EventBus.OnStateChanged?.Invoke();
        EventBus.OnItemsChanged?.Invoke(id);

        ClearCache();
        return true;
    }

    public int GetCurrentCoinValue(string id)
    {
        if (_cachedCoinIncome.TryGetValue(id, out int value) == false)
        {
            value = RecountConValue(id);
            _cachedCoinIncome[id] = value;
        }
        return value;
    }

    private int RecountConValue(string id)
    {
        var config = _gameConfig.GetItemConfig(id);
        float baseVal = config.BaseIncome;

        float flatBonus = GetAddIncome();  
        float multiplier = GetMultiplier();
        
        return Mathf.FloorToInt((baseVal + flatBonus) * ((multiplier+100)/100));
        
        float GetAddIncome()
        {
            float total = 0;
            if (config.Upgrades != null && config.Upgrades.Count > 0)
            {
                var upgrade = config.Upgrades[0];
                int level = _gameState.GetItemLevel(upgrade.Id);
                total += upgrade.Add*level;
            }
            return total;
        }

        float GetMultiplier()
        {
            float total = 0;
            if (config.Upgrades != null && config.Upgrades.Count > 1)
            {
                var upgrade = config.Upgrades[1];
                int level = _gameState.GetItemLevel(upgrade.Id);
                total += upgrade.MultAdd*level;
            }
            return total;
        }
        
    }

    public float GetCurrentCoinMoveDuration(string id)
    {
        if (_cachedCoinMoveDur.TryGetValue(id, out float value) == false)
        {
            value = RecountCoinMoveDuration(id);
            _cachedCoinMoveDur[id] = value;
        }
        return value;
    }

    private float RecountCoinMoveDuration(string id)
    {
        var config = _gameConfig.GetItemConfig(id);
        float baseVal = config.BaseTossDuration;
        float total = baseVal;
        
        if (config.Upgrades != null && config.Upgrades.Count > 2)
        {
            var upgrade = config.Upgrades[2];
            int level = _gameState.GetItemLevel(upgrade.Id);
            var mathPow = Mathf.Pow(1-upgrade.MoveDurMinus, level);
            total = baseVal*mathPow;
        }
        return total;
    }

    public float GetCurrentDoubleFlipChance(string id)
    {
        if (_cachedCoinDoubleFlipChance.TryGetValue(id, out float value) == false)
        {
            value = RecountDoubleFlipChance(id);
            _cachedCoinDoubleFlipChance[id] = value;
        }
        Debug.Log(value);
        return value;
    }
    public float RecountDoubleFlipChance(string id)
    {
        var config = _gameConfig.GetItemConfig(id);
        float total = 0;
        if (config.Upgrades != null && config.Upgrades.Count > 3)
        {
            var upgrade = config.Upgrades[3];
            int level = _gameState.GetItemLevel(upgrade.Id);
            total = config.Upgrades[3].DoubleFlipChance*level;
        }
        return total;
    }

    public float GetCurrentHelperSpeed(string id)
    {
        var value = _cachedChelSpeedMoveDur;
        if (value == 0)
        {
            var bms = _gameConfig.GetItemConfig(id).BaseMoveSpeed;
            var upgLevel = _gameState.GetItemLevel("chel_upgSpeedAdd");
            value = bms+(bms*_gameConfig.GetItemConfig("chel_upgSpeedAdd").MoveAdd*upgLevel/100);
            _cachedChelSpeedMoveDur = value;
        }
        return value;
    }









    #if UNITY_EDITOR 
    [ContextMenu("AddDolllars5000")]
    public void AddDollars()
    {
        TryToAddDollars(5000);
    }

    [ContextMenu("AddDolllars50000")]
    public void AddDollars2()
    {
        TryToAddDollars(50000);
    }

    [ContextMenu("AddDolllars500000000")]
    public void AddDollars3()
    {
        TryToAddDollars(500000000);
    }

    
#endif
}
