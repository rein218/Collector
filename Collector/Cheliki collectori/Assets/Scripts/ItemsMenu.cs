using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class ItemsMenu : MonoBehaviour
{
    public static ItemsMenu instance;
    [SerializeField] private bool load = false;
    [SerializeField] private bool SetDefault = false;
    [SerializeField] private float saveTimer = 15;
    private float _currentsaveTimer = 15;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform  containerItems,
                                        containerFeatures,
                                        containerUpgrades;
    [SerializeField] private List<ItemSorter> sorters; 
    [SerializeField] private List<ItemData> itemsData,
                                            featuresData;
    [SerializeField] private List<ItemUpgradeData> upgradesData;
    private Dictionary<ItemData, ItemButton> buttonObjectsForItems = new Dictionary<ItemData, ItemButton>();

    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private Spawner spawner;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private BusChelixCoins busChelixCoins;

    public void Awake()
    {
        if(instance == null)
        instance = this;
        itemsData.Clear();
        featuresData.Clear();
        upgradesData.Clear();

        var items = Resources.LoadAll<ItemData>("ScriptableObjects/Items");
        var feat = Resources.LoadAll<ItemData>("ScriptableObjects/Features");
        var upgr = Resources.LoadAll<ItemUpgradeData>("ScriptableObjects/Upgrades");


        if (itemsData == null) itemsData = new List<ItemData>();
        itemsData = items.ToList();
        if (featuresData == null) featuresData = new List<ItemData>();
        featuresData = feat.ToList();
        if (upgradesData == null) upgradesData = new List<ItemUpgradeData>();
        upgradesData = upgr.ToList();

    }

    public void Start()
    {
        if (SetDefault) SetDefaultSaves();
        if (load) Load();

        try {
        foreach (ItemData itemData in itemsData)
        {
            try
            {
                AddNewItem(itemData, containerItems, out GameObject gameObject);
            }
            catch
            {
                 Debug.Log("error with "+ itemData.name);
            }
            
            //костыль
            try
            {
                sorters[0].AddItem(itemData,gameObject);
            }
            catch
            {
                Debug.Log("error with sorter sorters[0]");
            }
            
        }}
        catch
            {
                Debug.Log("error with itemData");
            }

        foreach (ItemData upgradeData in upgradesData)
        {
            AddNewItem(upgradeData, containerUpgrades, out GameObject gameObject);
            //костыль
            sorters[1].AddItem(upgradeData,gameObject);
        }

        foreach (ItemData featureData in featuresData)
        {
            AddNewItem(featureData, containerFeatures, out GameObject gameObject);
            //костыль
            sorters[2].AddItem(featureData,gameObject);
        }

        if (load) LoadGameStuff();


        StartCoroutine(SaveCycle());

        SelectTab(containerItems.gameObject);
    }
    [ContextMenu("SetDefaultSaves")]
    public void SetDefaultSaves()
    {
        YG2.SetDefaultSaves();
        YG2.saves.SetDefault();
        YG2.SaveProgress();
    }

    IEnumerator SaveCycle()
    {
        _currentsaveTimer = saveTimer;
        while (true)
        {
            _currentsaveTimer-=Time.deltaTime;
            yield return null;
            if (_currentsaveTimer<0)
            {
                Save();
            }
        }
    }
    
    public void Save()
    {
        _currentsaveTimer = saveTimer;
        YG2.saves.itemsData = new List<SaveData>();
        foreach (var itemData in itemsData)
        {
            var data = itemData.GetSaveData();
            YG2.saves.itemsData.Add(data);
        }

        YG2.saves.featuresData = new List<SaveData>();
        foreach (var itemData in featuresData)
        {
            var data = itemData.GetSaveData();
            YG2.saves.featuresData.Add(data);
        }

        YG2.saves.itemsUpdateData = new List<SaveUpgradeData>();
        foreach (var upgradData in upgradesData)
        {
            var data = upgradData.GetSaveUpgradeData(); 
            YG2.saves.itemsUpdateData.Add(data);
        }

        int score;
        if(YG2.saves.allTimeDollarsCount>Int32.MaxValue)
        {
            score=Int32.MaxValue;
        }
        else
        {
            score=Convert.ToInt32(YG2.saves.allTimeDollarsCount);
        }

        YG2.SetLeaderboard("BestScore", score);
        YG2.SaveProgress();
    }

    public void Load()
    {
        if (YG2.saves == null || YG2.saves.isThisAFirstStart)
        {
            YG2.saves.SetDefault();
            YG2.saves.isThisAFirstStart = false;
            var coin = spawner.SpawnStartCoin(itemsData.FirstOrDefault(item => item.ItemType == ItemName.NewCoinBronze));;
            CameraController.instance.FirstStart(coin);
        }
        else
        {
            CameraController.instance.NormalStart();
            spawner.SpawnStartCoin(itemsData.FirstOrDefault(item => item.ItemType == ItemName.NewCoinBronze));;
        }

        int id = 0;
            foreach (var singleData in YG2.saves.itemsData)
            {
                itemsData[id].LoadSaveData(singleData);
                id++;
            }

            id = 0;
            foreach (var singleData in YG2.saves.featuresData)
            {
                featuresData[id].LoadSaveData(singleData);
                id++;
            }

            id = 0;
            foreach (var singleData in YG2.saves.itemsUpdateData)
            {
                upgradesData[id].LoadSaveUpgradeData(singleData);
                id++;
            }

        
    }
    public void LoadGameStuff()
    {
        if (YG2.saves == null || YG2.saves.itemsData == null || YG2.saves.itemsUpdateData == null) return;
        
        var linkToBronze = itemsData.FirstOrDefault(coin => coin.ItemType == ItemName.NewCoinBronze);
        for (int i = 0; i < YG2.saves.bronzeCount; i++)
        {
            spawner.SpawnNewCoin(linkToBronze, false);
        }

        var linkToSilver = itemsData.FirstOrDefault(coin => coin.ItemType == ItemName.NewCoinSilver);
        for (int i = 0; i < YG2.saves.siverCount; i++)
        {
            spawner.SpawnNewCoin(linkToSilver, false);
        }

        var linkToGold = itemsData.FirstOrDefault(coin => coin.ItemType == ItemName.NewCoinGold);
        for (int i = 0; i < YG2.saves.goldCount; i++)
        {
            spawner.SpawnNewCoin(linkToGold, false);
        }

        for (int i = 0; i < YG2.saves.chelixCount; i++)
        {
            spawner.SpawnNewChelix(false);
        }
        
    }
    
    //тоже костыль
    private void AddNewItem(ItemData itemData, Transform containerT) =>AddNewItem(itemData, containerT);
    private void AddNewItem(ItemData itemData, Transform containerT, out GameObject gameObject)
    {
        GameObject newItemButtonGO = Instantiate(itemButtonPrefab, containerT);
        ItemButton newItemButton = newItemButtonGO.GetComponent<ItemButton>();
        
        try
        {
            if(UnlockOnClick3(itemData)!=null) itemData.Init(ActionOnClick(itemData), UnlockOnClick(itemData), UnlockOnClick2(itemData), UnlockOnClick3(itemData));
            else if(UnlockOnClick2(itemData)!=null) itemData.Init(ActionOnClick(itemData), UnlockOnClick(itemData), UnlockOnClick2(itemData));
            else itemData.Init(ActionOnClick(itemData), UnlockOnClick(itemData));
        }
        catch
        {
            Debug.Log("AddNewItem1");
        }

       
        

        newItemButton.Init(itemData);
        buttonObjectsForItems.Add(itemData, newItemButton);

        //костыль
        gameObject = newItemButtonGO;
    }

    private UnityAction ActionOnClick(ItemData itemData)
    {
        try
        {
            
        
        switch (itemData.ItemType)
        {
            case ItemName.NewCoinBronze:
                return () => spawner.SpawnNewCoin(itemData);
            case ItemName.NewCoinSilver:
                return () => spawner.SpawnNewCoin(itemData);
            case ItemName.NewCoinGold:
                return () => spawner.SpawnNewCoin(itemData);
            
            case ItemName.NewChelix:
                
                return () => spawner.SpawnNewChelix();

            case ItemName.UpgradeCoinBronzeValue:   // upgrades are set in  ItemUpgradeData.UpgradeItem()
            case ItemName.UpgradeCoinSilverValue:
            case ItemName.UpgradeCoinGoldValue:
                return () => busChelixCoins.SetAllCoinsXValue(((ItemUpgradeData)itemData).ItemDataToUpgrade);
            case ItemName.UpgradeCoinBronzeValueM:   // upgrades are set in  ItemUpgradeData.UpgradeItem()
            case ItemName.UpgradeCoinSilverValueM:
            case ItemName.UpgradeCoinGoldValueM:
                return () => busChelixCoins.SetAllCoinsXValueM(((ItemUpgradeData)itemData).ItemDataToUpgrade);


            case ItemName.UpgradeCoinMoveDurationBronze:
            case ItemName.UpgradeCoinMoveDurationSilver:
            case ItemName.UpgradeCoinMoveDurationGold:
                return () => busChelixCoins.SetAllCoinsMoveDuration(((ItemUpgradeData)itemData).ItemDataToUpgrade, GetCurrentUpgrade(itemData.ItemType));

            case ItemName.UpgradeChelixSpeed:
                return () => busChelixCoins.SetSpeedOfAllChelix(((ItemUpgradeData)itemData).ItemDataToUpgrade);

            case ItemName.FeatureMouseHover:
                return () => inputHandler.SetClickNotRequired();

            case ItemName.FeatureUnlockCoinSilverForChelix:
                return () => busChelixCoins.UnlockTypeOfCoinForChelix(ItemName.NewCoinSilver);
            case ItemName.FeatureUnlockCoinGoldForChelix:
                return () => busChelixCoins.UnlockTypeOfCoinForChelix(ItemName.NewCoinGold);
        }}
        catch
        {
            Debug.Log("ActionOnClick");
        }
        return null;
    }

    private UnityAction UnlockOnClick(ItemData itemData)
    {
        try {
        switch (itemData.ItemType)
        {
            case ItemName.NewCoinBronze:
                return () => UnlockUpgrade(ItemName.UpgradeCoinBronzeValue, itemData.CurrentLevelOfUpgrade);

            case ItemName.NewCoinSilver:
                return () => UnlockUpgrade(ItemName.UpgradeCoinSilverValue, itemData.CurrentLevelOfUpgrade);

            case ItemName.NewCoinGold:
                return () => UnlockUpgrade(ItemName.UpgradeCoinGoldValue, itemData.CurrentLevelOfUpgrade);

            case ItemName.NewChelix:
                return () => UnlockUpgrade(ItemName.UpgradeChelixSpeed, itemData.CurrentLevelOfUpgrade);

        }}
        catch
        {
            Debug.Log("UnlockOnClick");
        }
        return null;
    }

    private UnityAction UnlockOnClick2(ItemData itemData)
    {
        try {
        switch (itemData.ItemType)
        {
            case ItemName.NewCoinBronze:
                return () => UnlockUpgrade(ItemName.UpgradeCoinBronzeValueM, itemData.CurrentLevelOfUpgrade);

            case ItemName.NewCoinSilver:
                return () => UnlockUpgrade(ItemName.UpgradeCoinSilverValueM, itemData.CurrentLevelOfUpgrade);

            case ItemName.NewCoinGold:
                return () => UnlockUpgrade(ItemName.UpgradeCoinGoldValueM, itemData.CurrentLevelOfUpgrade);
        }}
        catch
        {
            Debug.Log("UnlockOnClick2");
        }
        return null;
    }

    private UnityAction UnlockOnClick3(ItemData itemData)
    {
        try {
        switch (itemData.ItemType)
        {
            case ItemName.NewCoinBronze:
                return () => UnlockUpgrade(ItemName.UpgradeCoinMoveDurationBronze, itemData.CurrentLevelOfUpgrade);

            case ItemName.NewCoinSilver:
                return () => UnlockUpgrade(ItemName.UpgradeCoinMoveDurationSilver, itemData.CurrentLevelOfUpgrade);

            case ItemName.NewCoinGold:
                return () => UnlockUpgrade(ItemName.UpgradeCoinMoveDurationGold, itemData.CurrentLevelOfUpgrade);
        }}
        catch
        {
            Debug.Log("UnlockOnClick2");
        }
        return null;
    }
    

    public float GetCurrentUpgrade(ItemName upgradeName)
    {
        ItemUpgradeData upgradeForRequiredItemData = upgradesData.FirstOrDefault(upgr => upgr.ItemType == upgradeName);


        return upgradeForRequiredItemData.SpecialModifier * upgradeForRequiredItemData.CurrentLevelOfUpgrade;
    }

    public float GetCurrentCoinTypeMoveDuration(ItemName coinType)
    {
        switch (coinType)
        {
            case ItemName.NewCoinBronze:
                return GetCurrentUpgrade(ItemName.UpgradeCoinMoveDurationBronze);

            case ItemName.NewCoinSilver:
                return GetCurrentUpgrade(ItemName.UpgradeCoinMoveDurationSilver);

            case ItemName.NewCoinGold:
                return GetCurrentUpgrade(ItemName.UpgradeCoinMoveDurationGold);
        }
        return -1;
    }

    private void UnlockUpgrade(ItemName nameOfUnlocked, int upgrValue)
    {
        List<ItemUpgradeData> upgradesToUnlock = upgradesData.FindAll(upgr => upgr.ItemType == nameOfUnlocked);

        foreach (ItemUpgradeData upgradeToUnlock in upgradesToUnlock)
        {
            if (upgradeToUnlock != null)
            {
                if (upgradeToUnlock.TryToUnlock(upgrValue))
                    buttonObjectsForItems[upgradeToUnlock].gameObject.SetActive(true);
                //Debug.Log($"Upgrade with name {upgradeToUnlock.ItemType} na  {upgrValue} da {upgradeToUnlock.IsUnlocked}");
            }
            else
            {
                Debug.LogError($"Upgrade with name {nameOfUnlocked} not found in upgradesData list");
            }
        }
    }

    public void SelectTab(GameObject tabToOpen)
    {
        containerItems.gameObject.SetActive(false);
        containerFeatures.gameObject.SetActive(false);
        containerUpgrades.gameObject.SetActive(false);

        tabToOpen.SetActive(true);
        scrollRect.content = tabToOpen.GetComponent<RectTransform>();
    }
}
