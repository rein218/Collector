using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        foreach (ItemData itemData in itemsData)
        {
            AddNewItem(itemData);
        }
        // maybe should add new tab for upgrades
        foreach (ItemData upgradeData in upgradesData)
        {
            AddNewItem(upgradeData);
        }
        //
    }
    
    public void TestSave()
    {
        var test1 = new TestClass("first", true);
        var test2 = new TestClass("second", false);
        var test3 = new TestClass("third", false);


        YG2.saves.testsData = new List<TestClass>();
        var data = new TestClass();

        data.CopyFrom(test1);
        YG2.saves.testsData.Add(data);

        data.CopyFrom(test2);
        YG2.saves.testsData.Add(data);

        data.CopyFrom(test3);
        YG2.saves.testsData.Add(data);

        YG2.SaveProgress();

        string json = JsonUtility.ToJson(YG2.saves);
         Debug.Log(json);
        PlayerPrefs.SetString("SaveData", json);

        PlayerPrefs.Save();
    }

    public void TestLoad()
    {
        Debug.Log(YG2.saves.testsData.Count);
        foreach (var singleData in YG2.saves.testsData)
        {
            var data = new TestClass();
            data.CopyFrom(singleData);
            Debug.Log(data.itemName + " " + data.isUnlocked);
        }
    }
    
    public void Save()
    {

        YG2.saves.itemsData = new List<ItemData>();
        foreach (var itemData in itemsData)
        {
            var data = ScriptableObject.CreateInstance<ItemData>();;
            data.CopyFrom(itemData);
            Debug.Log(data.ItemName);
            YG2.saves.itemsData.Add(data);
        }
        YG2.SaveProgress();

         
    }

    public void Load()
    {
        itemsData = new List<ItemData>();
        Debug.Log(YG2.saves.itemsData.Count);
        foreach (var singleData in YG2.saves.itemsData)
        {
            var data = ScriptableObject.CreateInstance<ItemData>();
            if (singleData == null)
            {
                Debug.LogError("хуйня");
                return;
            }
            Debug.Log(singleData.ItemName);
            data.CopyFrom(singleData);
            itemsData.Add(data);
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

            case ItemName.FeatureMouseHover:
                return () => inputHandler.SetClickNotRequired();

            case ItemName.UpgradeCoinBronzeValue:
                return () => busChelixCoins.SetAllCoinsBronzeValue(((ItemUpgradeData)itemData).ItemDataToUpgrade);

            case ItemName.UpgradeChelixSpeed:
                return () => busChelixCoins.SetSpeedOfAllChelix(((ItemUpgradeData)itemData).ItemDataToUpgrade);

            // upgrades are set in  ItemUpgradeData.UpgradeItem()
        }
        //Save();
        return null;
    }

    private UnityAction UnlockOnClick(ItemData itemData)
    {
        switch (itemData.ItemName)
        {
            case ItemName.NewCoinBronze:
            case ItemName.NewCoinSilver:
            case ItemName.NewCoinGold:
                return () => UnlockUpgrade(ItemName.UpgradeCoinBronzeValue, itemData.UpgradeCurrentValue);

            case ItemName.NewChelix:
                return () => UnlockUpgrade(ItemName.UpgradeChelixSpeed, itemData.UpgradeCurrentValue);
        }

        return null;
    }

    private void UnlockUpgrade(ItemName nameOfUnlocked, int upgrValue)
    {
        ItemUpgradeData upgradeToUnlock = upgradesData.First(upgr => upgr.ItemName == nameOfUnlocked);

        upgradeToUnlock?.Unlock(upgrValue);
    }
}

[Serializable]
public class TestClass 
{
    public bool isUnlocked;
    public string itemName;
    public TestClass()
    {
        
    }
    public TestClass(string itemName, bool isUnlocked)
    {
        this.itemName = itemName;
        this.isUnlocked = isUnlocked;
    }

    public void CopyTo(TestClass toWhom)
    {
        toWhom.isUnlocked = this.isUnlocked;
        toWhom.itemName = this.itemName;
    }

    public void CopyFrom(TestClass fromWhom)
    {
        this.isUnlocked = fromWhom.isUnlocked;
        this.itemName = fromWhom.itemName;
    }
}