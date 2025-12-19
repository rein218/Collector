using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ItemData
{
    [SerializeField] private string itemName;
        public string ItemName => itemName;
    [SerializeField] private  Sprite sprite;
        public Sprite Sprite => sprite;
    [SerializeField] private  int priceDefault;
        public int PriceDefault => priceDefault;
    [SerializeField] private  int priceCurrent;
        public int PriceCurrent => priceCurrent;
    [SerializeField] private  int priceModifierOnUpgrade;
        public int PriceModifierOnUpgrade => priceModifierOnUpgrade;
    [SerializeField] private  int upgradeMaxValue;
        public int UpgradeMaxValue => upgradeMaxValue;
    [SerializeField] private  int upgradeCurrentValue;
        public int UpgradeCurrentValue => upgradeCurrentValue;


    public bool Upgrade()
    {
        if (IsUpgradedFull()) return false;

        upgradeCurrentValue++;

        priceCurrent += priceModifierOnUpgrade;

        return true;
    }

    public bool IsUpgradedFull()
    {
        return upgradeCurrentValue >= upgradeMaxValue;
    }
}
