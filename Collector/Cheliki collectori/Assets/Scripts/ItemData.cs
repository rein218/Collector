using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    protected bool isUnlocked;
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
    public UnityAction unlockOnClick {get; protected private set; }

    virtual public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        isUnlocked = true;
        priceCurrent = priceDefault;
        upgradeCurrentValue = 0;
        specialCurrentValue = specialDefaultValue;

        Debug.Log($"specialCurrentValue == {specialCurrentValue}");

        actionOnClick = newActionOnClick;
        unlockOnClick = newUnlockOnClick;
    }


    public bool ButtonClick()
    {
        if (IsUpgradedFull() || !CurrenciesWallet.Instance.SpendDollars(PriceCurrent) || !isUnlocked) return false;

        IncreaseUpgradeCurrentValue();
        IncreasePriceCurrentValue();

        actionOnClick?.Invoke();
        unlockOnClick?.Invoke();

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

    public void CopyTo(ItemData toWhom)
    {
        toWhom.isUnlocked = this.isUnlocked;
        toWhom.itemName = this.itemName;
        toWhom.sprite = this.sprite;
        toWhom.priceDefault = this.priceDefault;
        toWhom.priceCurrent = this.priceCurrent;
        toWhom.priceModifierOnUpgrade = this.priceModifierOnUpgrade;
        toWhom.upgradeMaxValue = this.upgradeMaxValue;
        toWhom.upgradeCurrentValue = this.upgradeCurrentValue;
        toWhom.specialDefaultValue = this.specialDefaultValue;
        toWhom.specialCurrentValue = this.specialCurrentValue;
    }

    public void CopyFrom(ItemData fromWhom)
    {
        this.isUnlocked = fromWhom.isUnlocked;
        this.itemName = fromWhom.itemName;
        this.sprite = fromWhom.sprite;
        this.priceDefault = fromWhom.priceDefault;
        this.priceCurrent = fromWhom.priceCurrent;
        this.priceModifierOnUpgrade = fromWhom.priceModifierOnUpgrade;
        this.upgradeMaxValue = fromWhom.upgradeMaxValue;
        this.upgradeCurrentValue = fromWhom.upgradeCurrentValue;
        this.specialDefaultValue = fromWhom.specialDefaultValue;
        this.specialCurrentValue = fromWhom.specialCurrentValue;
    }
}
