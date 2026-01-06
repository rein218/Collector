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
            case ItemName.NewCoin:
                return () => spawner.SpawnNewCoin(itemData);

            case ItemName.NewChelix:
                return () => spawner.SpawnNewChelix();
            case ItemName.UnlockMouseHover:
                return () => inputHandler.SetClickNotRequired();

                // upgrades are set in ItemUpgradeData
        }

        return null;
    }

    private UnityAction UnlockOnClick(ItemData itemData)
    {
        switch (itemData.ItemName)
        {
            case ItemName.NewCoin:
                return () => UnlockUpgrade(ItemName.UpgradeCoinValue, itemData.UpgradeCurrentValue);

        }

        return null;
    }

    private void UnlockUpgrade(ItemName nameOfUnlocked, int upgrValue)
    {
        ItemUpgradeData upgradeToUnlock = upgradesData.First(upgr => upgr.ItemName == nameOfUnlocked);

        upgradeToUnlock?.Unlock(upgrValue);
    }
}