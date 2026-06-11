using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GameObject purchaseButtonPrefab; // единый префаб кнопки
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private Transform upgradesContainer;
    [SerializeField] private Transform featuresContainer;
    [SerializeField] private GameObject itemsRect;    

    [SerializeField] private GameObject upgradesRect;    
    [SerializeField] private GameObject featuresRect;


    // Храним кнопки (ItemButton) для быстрого доступа
    private Dictionary<ShopItemConfig, ItemButton> items = new Dictionary<ShopItemConfig, ItemButton>();
    private Dictionary<ShopItemConfig, ItemButton> upgrades = new Dictionary<ShopItemConfig, ItemButton>();
    private Dictionary<ShopItemConfig, ItemButton> features = new Dictionary<ShopItemConfig, ItemButton>();

    // Сохраняем исходный порядок конфигов
    private List<ShopItemConfig> orderedItems = new List<ShopItemConfig>();
    private List<ShopItemConfig> orderedUpgrades = new List<ShopItemConfig>();
    private List<ShopItemConfig> orderedFeatures = new List<ShopItemConfig>();



    public void Start() 
    {
        itemsRect.SetActive(false);
        upgradesRect.SetActive(false);
        featuresRect.SetActive(false);
        CreateButtons();
        itemsRect.SetActive(true);
    }
    

    void OnEnable() => EventBus.OnStateChanged += Sort;
    void OnDisable() => EventBus.OnStateChanged -= Sort;

    private void CreateButtons()
    {
        foreach (var itemConfig in _gameConfig.allItems)
        {
            ItemButton btn = null;
            switch (itemConfig.shopTab)
            {
                case ShopTab.Item:
                    btn = CreateItemButton(itemConfig, itemsContainer);
                    items.Add(itemConfig, btn);
                    orderedItems.Add(itemConfig);
                    break;
                case ShopTab.Upgrade:
                    btn = CreateItemButton(itemConfig, upgradesContainer);
                    upgrades.Add(itemConfig, btn);
                    orderedUpgrades.Add(itemConfig);
                    break;
                default: // Features
                    btn = CreateItemButton(itemConfig, featuresContainer);
                    features.Add(itemConfig, btn);
                    orderedFeatures.Add(itemConfig);
                    break;
            }
        }
        Sort();
    }

    private ItemButton CreateItemButton(ShopItemConfig itemConfig, Transform container)
    {
        var obj = Instantiate(purchaseButtonPrefab, container);
        var btn = obj.GetComponent<ItemButton>();
        btn.Init(itemConfig);
        return btn;
    }

    public void Sort()
    {
        SortList(orderedItems, items, itemsContainer);
        SortList(orderedUpgrades, upgrades, upgradesContainer);
        SortList(orderedFeatures, features, featuresContainer);
    }

    private void SortList(List<ShopItemConfig> orderedConfigs, Dictionary<ShopItemConfig, ItemButton> dict, Transform container)
    {
        // Разделяем на разблокированные и заблокированные, сохраняя исходный порядок
        var unlocked = new List<ItemButton>();
        var locked = new List<ItemButton>();
        foreach (var config in orderedConfigs)
        {
            if (dict.TryGetValue(config, out var btn))
            {
                if (btn.IsVisible()) unlocked.Add(btn);
                else locked.Add(btn);
            }
        }

        // Объединяем: сначала разблокированные, потом заблокированные
        var sorted = new List<ItemButton>(unlocked);
        sorted.AddRange(locked);

        // Применяем порядок через sibling index
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].transform.SetSiblingIndex(i);
        }
    }
}