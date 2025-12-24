using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] protected private ItemName itemName;
        public ItemName ItemName => itemName;
    [SerializeField] protected private  Sprite sprite;
        public Sprite Sprite => sprite;


    [SerializeField] protected private  int priceDefault;
        public int PriceDefault => priceDefault;
    protected private  int priceCurrent;
        public int PriceCurrent => priceCurrent;
    [SerializeField] protected private  int priceModifierOnUpgrade;
        public int PriceModifierOnUpgrade => priceModifierOnUpgrade;
    [SerializeField] protected private  int upgradeMaxValue;
        public int UpgradeMaxValue => upgradeMaxValue;
    protected private  int upgradeCurrentValue;
        public int UpgradeCurrentValue => upgradeCurrentValue;


    [SerializeField] private int specialDefaultValue;
        public int SpecialDefaultValue => specialDefaultValue;
    private  int specialCurrentValue;
        public int SpecialCurrentValue => specialCurrentValue;


    public UnityAction actionOnClick {get; protected private set; }
    

    virtual public void Init (UnityAction newActionOnClick)
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
    public void IncreaseSpecialCurrentValue(int specialModifierValue)
    {
        specialCurrentValue += specialModifierValue;

        Debug.Log("specialCurrentValue");
    }

    public bool IsUpgradedFull()
    {
        return upgradeCurrentValue >= upgradeMaxValue;
    }
}
