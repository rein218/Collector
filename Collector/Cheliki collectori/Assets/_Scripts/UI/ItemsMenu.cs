using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsMenu : MonoBehaviour
{
    //косл
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GameObject purchaseButtonPrefab;    // префаб кнопки покупки предмета
    [SerializeField] private GameObject upgradeButtonPrefab;     // префаб кнопки улучшения
    [SerializeField] private Transform itemsContainer;           // родитель для кнопок предметов
    [SerializeField] private Transform upgradesContainer;        // родитель для кнопок улучшений
    public void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        foreach (var itemConfig in _gameConfig.allItems)
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
