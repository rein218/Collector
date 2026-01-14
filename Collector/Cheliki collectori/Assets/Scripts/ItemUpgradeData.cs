using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemUpgradeData")]
public class ItemUpgradeData : ItemData
{
    public SaveUpgradeData GetSaveUpgradeData() => new SaveUpgradeData
    {
        saveData = base.GetSaveData(),
        specialModifier = _specialModifier,
        valueToUnlock = _valueToUnlock,
    };
    
    public void LoadSaveUpgradeData(SaveUpgradeData data)
    {
        base.LoadSaveData(data.saveData);
        _specialModifier = data.specialModifier;
        _valueToUnlock =  data.valueToUnlock;
    }
    
    [SerializeField] private ItemData itemDataToUpgrade;
    public ItemData ItemDataToUpgrade => itemDataToUpgrade;
    [SerializeField] private float _specialModifier;
        public float SpecialModifier => _specialModifier;

    [SerializeField] private int _valueToUnlock;

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
        if (upgrValue == _valueToUnlock) _isUnlocked = true;
    }


}
