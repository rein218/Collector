using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameConfig _gameConfig;
    private GameState _gameState;

    private void Awake()
    {
        if(instance == null)
        instance = this; 

        _gameState = new GameState();
        InitializeNewGameState();
    }
    private void InitializeNewGameState()
    {
        foreach (var itemConfig in _gameConfig.GetAllItems())
        {
            var itemState = new ItemState
            {
                itemType = itemConfig.itemType,
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
    }

    public bool InEnough(int dollarToSpend)
    {
        if (_gameState.dollarsCount >= dollarToSpend)
        {
            return true;
        }
        return false;
    }

    #if UNITY_EDITOR 
    [ContextMenu("AddDolllars")]
    public void AddDollars()
    {
        TryToAddDollars(5000);
    }

    [ContextMenu("AddDolllars2")]
    public void AddDollars2()
    {
        TryToAddDollars(50000);
    }

    [ContextMenu("AddDolllars2")]
    public void AddDollars3()
    {
        TryToAddDollars(500000000);
    }
    #endif

    
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

    public bool IsItemUnlocked(ItemName itemName)
    {
        return true;
    }

    public float GetActualPrice(ItemName itemName)
    {
        ItemState state = _gameState.GetItemState(itemName);
        var itemConfig = _gameConfig.GetItemConfig(itemName);
        float cost = itemConfig.BaseCost +(itemConfig.CostAdd * Mathf.Pow(itemConfig.CostAddMultiplier, state.upgradeLevel));
        return cost;
    }
}
