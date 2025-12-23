using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ItemData
{
    [SerializeField] private ItemName itemName;
        public ItemName ItemName => itemName;
    [SerializeField] private  Sprite sprite;
        public Sprite Sprite => sprite;


    [SerializeField] private  int priceDefault;
        public int PriceDefault => priceDefault;
    private  int priceCurrent;
        public int PriceCurrent => priceCurrent;
    [SerializeField] private  int priceModifierOnUpgrade;
        public int PriceModifierOnUpgrade => priceModifierOnUpgrade;
    [SerializeField] private  int upgradeMaxValue;
        public int UpgradeMaxValue => upgradeMaxValue;
    private  int upgradeCurrentValue;
        public int UpgradeCurrentValue => upgradeCurrentValue;


    [SerializeField] private  int specialDefaultValue;
        public int SpecialDefaultValue => specialDefaultValue;
    private  int specialCurrentValue;
        public int SpecialCurrentValue => specialCurrentValue;
    [SerializeField] private int specialModifier;
        public int SpecialModifier => specialModifier;

    public UnityAction actionOnClick {get; private set; }
    

    public void Init(UnityAction newActionOnClick )
    {
        priceCurrent = priceDefault;
        upgradeCurrentValue = 0;
        specialCurrentValue = specialDefaultValue;
        Debug.Log($"specialCurrentValue == {specialCurrentValue}");

        actionOnClick = newActionOnClick;
    }


    public bool ButtonClick()
    {
        if (IsUpgradedFull() || !CurrenciesWallet.Instance.SpendDollars(PriceCurrent)) return false;

        IncreaseUpgradeCurrentValue();
        IncreasePriceCurrentValue();

        actionOnClick?.Invoke();

        return true;
    }

    public void IncreaseUpgradeCurrentValue()
    {
        upgradeCurrentValue++;
    }
    public void IncreasePriceCurrentValue()
    {
        priceCurrent += priceModifierOnUpgrade;
    }
    public void IncreaseSpecialCurrentValue()
    {
        specialCurrentValue += specialModifier;
    }

    public bool IsUpgradedFull()
    {
        return upgradeCurrentValue >= upgradeMaxValue;
    }
}
