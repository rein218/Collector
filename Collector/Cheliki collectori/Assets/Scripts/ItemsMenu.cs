using UnityEngine;
using UnityEngine.Events;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform containerT;
    [SerializeField] private ItemData[] itemsData;

    [SerializeField] private Spawner spawner;

    public void Awake()
    {
        foreach (ItemData itemData in itemsData)
        {
            AddNewItem(itemData);
        }
    }
    

    private void AddNewItem(ItemData itemData)
    {
        GameObject newItemButtonGO = Instantiate(itemButtonPrefab, containerT);
        ItemButton newItemButton = newItemButtonGO.GetComponent<ItemButton>();

        itemData.Init(ActionOnClick(itemData));

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

            // upgrades are set in ItemUpgradeData
        }

        return null;
    }
}