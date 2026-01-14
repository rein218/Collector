using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform containerT;
    [SerializeField] private List<ItemData> itemsData;
    [SerializeField] private List<ItemUpgradeData> upgradesData;

    [SerializeField] private Spawner spawner;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private BusChelixCoins busChelixCoins;

    [Order(-1)]
    public void Start()
    {
        Load();
        foreach (ItemData itemData in itemsData)
        {
            AddNewItem(itemData);
        }

        foreach (ItemData upgradeData in upgradesData)
        {
            AddNewItem(upgradeData);
        }
        StartCoroutine(SaveCycle());
    }

    IEnumerator SaveCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            Save();
        }
    }
    
    public void Save()
    {
        YG2.saves.itemsData = new List<SaveData>();
        foreach (var itemData in itemsData)
        {
            var data = itemData.GetSaveData();
            Debug.Log("1");
            YG2.saves.itemsData.Add(data);
        }

        YG2.saves.itemsUpdateData = new List<SaveUpgradeData>();
        foreach (var upgradData in upgradesData)
        {
            var data = upgradData.GetSaveUpgradeData();
            Debug.Log("2");
            YG2.saves.itemsUpdateData.Add(data);
        }


        YG2.SaveProgress();
        Debug.Log("Saved: "+ JsonUtility.ToJson(YG2.saves, true));
    }

    public void Load()
    {
        if (YG2.saves == null || YG2.saves.itemsData == null || YG2.saves.itemsUpdateData == null) return;
        Debug.Log("Load: "+ JsonUtility.ToJson(YG2.saves, true));
        foreach (var singleData in YG2.saves.itemsData)
        {
            var data = new ItemData();
            data.LoadSaveData(singleData);
        }


        foreach (var singleData in YG2.saves.itemsUpdateData)
        {
            var data = new ItemUpgradeData();
            data.LoadSaveUpgradeData(singleData);
        }



    }
    

    private void AddNewItem(ItemData itemData)
    {
        GameObject newItemButtonGO = Instantiate(itemButtonPrefab, containerT);
        ItemButton newItemButton = newItemButtonGO.GetComponent<ItemButton>();

        itemData.Init(ActionOnClick(itemData), UnlockOnClick(itemData));

        newItemButton.Init(itemData);
    }

    private UnityAction ActionOnClick(ItemData itemData)
    {
        switch (itemData.ItemName)
        {
            case ItemName.NewCoinBronze:
            case ItemName.NewCoinSilver:
            case ItemName.NewCoinGold:
                return () => spawner.SpawnNewCoin(itemData);

            case ItemName.NewChelix:
                return () => spawner.SpawnNewChelix();

            case ItemName.UpgradeCoinBronzeValue:   // upgrades are set in  ItemUpgradeData.UpgradeItem()
            case ItemName.UpgradeCoinSilverValue:
            case ItemName.UpgradeCoinGoldValue:
                return () => busChelixCoins.SetAllCoinsXValue(((ItemUpgradeData)itemData).ItemDataToUpgrade);

            case ItemName.UpgradeCoinMoveDurationBronze:
            case ItemName.UpgradeCoinMoveDurationSilver:
            case ItemName.UpgradeCoinMoveDurationGold:
                return () => busChelixCoins.SetAllCoinsMoveDuration(((ItemUpgradeData)itemData).ItemDataToUpgrade, GetCurrentUpgrade(itemData.ItemName));

            case ItemName.UpgradeChelixSpeed:
                return () => busChelixCoins.SetSpeedOfAllChelix(((ItemUpgradeData)itemData).ItemDataToUpgrade);

            case ItemName.FeatureMouseHover:
                return () => inputHandler.SetClickNotRequired();

            case ItemName.FeatureUnlockCoinSilverForChelix:
                return () => busChelixCoins.UnlockTypeOfCoinForChelix(ItemName.NewCoinSilver);
            case ItemName.FeatureUnlockCoinGoldForChelix:
                return () => busChelixCoins.UnlockTypeOfCoinForChelix(ItemName.NewCoinGold);

        }
        return null;
    }

    private UnityAction UnlockOnClick(ItemData itemData)
    {
        switch (itemData.ItemName)
        {
            case ItemName.NewCoinBronze:
                return () => UnlockUpgrade(ItemName.UpgradeCoinBronzeValue, itemData.UpgradeCurrentValue);

            case ItemName.NewCoinSilver:
                return () => UnlockUpgrade(ItemName.UpgradeCoinSilverValue, itemData.UpgradeCurrentValue);

            case ItemName.NewCoinGold:
                return () => UnlockUpgrade(ItemName.UpgradeCoinGoldValue, itemData.UpgradeCurrentValue);

            case ItemName.NewChelix:
                return () => UnlockUpgrade(ItemName.UpgradeChelixSpeed, itemData.UpgradeCurrentValue);
        }

        return null;
    }

    public float GetCurrentUpgrade(ItemName upgradeName)
    {
        ItemUpgradeData upgradeForRequiredItemData = upgradesData.FirstOrDefault(upgr => upgr.ItemName == upgradeName);

        Debug.Log($"asqweqwesadczx: {upgradeForRequiredItemData} ; {upgradeName}");

        return upgradeForRequiredItemData.SpecialModifier * upgradeForRequiredItemData.UpgradeCurrentValue;
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
        List<ItemUpgradeData> upgradesToUnlock = upgradesData.FindAll(upgr => upgr.ItemName == nameOfUnlocked);

        foreach (ItemUpgradeData upgradeToUnlock in upgradesToUnlock)
        {
            if (upgradeToUnlock != null)
            {
                upgradeToUnlock.Unlock(upgrValue);
            }
            else
            {
                Debug.LogError($"Upgrade with name {nameOfUnlocked} not found in upgradesData list");
            }
        }
    }
}
