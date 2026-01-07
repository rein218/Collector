using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform containerT;
    [SerializeField] private ItemData[] itemsData;
    [SerializeField] private ItemUpgradeData[] upgradesData;

    [SerializeField] private Spawner spawner;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private BusChelixCoins busChelixCoins;

    public void Awake()
    {
        foreach (ItemData itemData in itemsData)
        {
            AddNewItem(itemData);
        }
        // maybe should add new tab for upgrades
        foreach (ItemData upgradeData in upgradesData)
        {
            AddNewItem(upgradeData);
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