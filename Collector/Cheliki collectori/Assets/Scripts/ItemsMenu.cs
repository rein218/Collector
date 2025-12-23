using UnityEngine;
using UnityEngine.Events;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform containerT;
    [SerializeField] private ItemSO[] itemsSO;

    [SerializeField] private Spawner spawner;

    public void Awake()
    {
        foreach (ItemSO itemSO in itemsSO)
        {
            AddNewItem(itemSO);
        }
    }
    

    private void AddNewItem(ItemSO itemSO)
    {
        GameObject newItemButtonGO = Instantiate(itemButtonPrefab, containerT);
        ItemButton newItemButton = newItemButtonGO.GetComponent<ItemButton>();

        itemSO.ItemData.Init(ActionOnClick(itemSO));

        newItemButton.Init(itemSO);
    }

    private UnityAction ActionOnClick(ItemSO itemSO)
    {
        switch (itemSO.ItemData.ItemName)
        {
            case ItemName.NewCoin:
                return () => spawner.SpawnNewCoin(itemSO);

            case ItemName.NewChelix:
                return () => spawner.SpawnNewChelix();
        }

        return () => Debug.LogError("ActionOnClick hasn't been set");
    }
}