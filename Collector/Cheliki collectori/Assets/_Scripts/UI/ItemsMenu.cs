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
    [SerializeField] private List<ItemConfig> allItemConfigs;
    [SerializeField] private List<UpgradeConfig> allUpgradeConfigs;
    [SerializeField] private GameObject purchaseButtonPrefab;    // префаб кнопки покупки предмета
    [SerializeField] private GameObject upgradeButtonPrefab;     // префаб кнопки улучшения
    [SerializeField] private Transform itemsContainer;           // родитель для кнопок предметов
    [SerializeField] private Transform upgradesContainer;        // родитель для кнопок улучшений
    private GameState gameState;

    public void Init(GameState gameState)
    {
        this.gameState = gameState;
        CreateButtons();
    }

    private void CreateButtons()
    {
        foreach (var itemConfig in allItemConfigs)
        {
            CreateItemButton(itemConfig);
        }
    }

    private void CreateItemButton(ItemConfig itemConfig)
    {
        var btn = Instantiate(purchaseButtonPrefab,itemsContainer);
        btn.GetComponent<ItemButton>().Init(itemConfig);
    }
}
