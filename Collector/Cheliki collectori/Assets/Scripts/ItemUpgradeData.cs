using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemUpgradeData")]
public class ItemUpgradeData : ItemData
{
    public SaveUpgradeData GetSaveData() => new SaveUpgradeData
    {
        isUnlocked = _isUnlocked,
        itemName = _itemName,
        sprite = _sprite,
        priceDefault = _priceDefault,
        priceCurrent = _priceCurrent,
        priceModifierOnUpgrade = _priceModifierOnUpgrade,
        upgradeMaxValue = _upgradeMaxValue,
        upgradeCurrentValue = _upgradeCurrentValue,
        specialDefaultValue = _specialDefaultValue,
        specialCurrentValue = _specialCurrentValue
    };
    
    public void LoadSaveData(SaveData data)
    {
        _isUnlocked = data.isUnlocked;
        _itemName = data.itemName;
        _sprite = data.sprite;
        _priceDefault = data.priceDefault;
        _priceCurrent = data.priceCurrent;
        _priceModifierOnUpgrade = data.priceModifierOnUpgrade;
        _upgradeMaxValue = data.upgradeMaxValue;
        _upgradeCurrentValue = data.upgradeCurrentValue;
        _specialDefaultValue = data.specialDefaultValue;
        _specialCurrentValue = data.specialCurrentValue;
    }
    
    [SerializeField] private ItemData itemDataToUpgrade;
    public ItemData ItemDataToUpgrade => itemDataToUpgrade;
    [SerializeField] private float _specialModifier;
        public float SpecialModifier => _specialModifier;

    [SerializeField] private int valueToUnlock;

    override public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        _isUnlocked = false;
        _priceCurrent = _priceDefault;
        _upgradeCurrentValue = 0;

        eventOnClick.AddListener(() => UpgradeItem());
        if (newActionOnClick != null) eventOnClick.AddListener(newActionOnClick);
    }

    public void UpgradeItem()
    {
        itemDataToUpgrade.IncreaseSpecialCurrentValue(_specialModifier);
    }

    public void Unlock(int upgrValue)
    {
        if (upgrValue == valueToUnlock) _isUnlocked = true;
    }


}
