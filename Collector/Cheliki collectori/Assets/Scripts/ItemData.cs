using System;
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


    [SerializeField] private float specialDefaultValue;
        public float SpecialDefaultValue => specialDefaultValue;
    private  float specialCurrentValue;
        public float SpecialCurrentValue => specialCurrentValue;


    public UnityEvent eventOnClick {get; protected private set; }

    virtual public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        isUnlocked = true;
        priceCurrent = priceDefault;
        upgradeCurrentValue = 0;
        specialCurrentValue = specialDefaultValue;

        Debug.Log($"specialCurrentValue == {specialCurrentValue}");

        eventOnClick.AddListener(newActionOnClick);
        eventOnClick.AddListener(newUnlockOnClick);
    }


    public bool ButtonClick()
    {
        if (IsUpgradedFull() || !CurrenciesWallet.Instance.SpendDollars(PriceCurrent) || !isUnlocked) return false;

        IncreaseUpgradeCurrentValue();
        IncreasePriceCurrentValue();

        eventOnClick?.Invoke();

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
    public void IncreaseSpecialCurrentValue(float specialModifierValue)
    {
        specialCurrentValue += specialModifierValue;

        Debug.Log("specialCurrentValue");
    }

    public bool IsUpgradedFull()
    {
        return upgradeCurrentValue >= upgradeMaxValue;
    }

    private void OnDestroy()
    {
        eventOnClick.RemoveAllListeners();
    }
}
