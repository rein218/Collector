using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemUpgradeData")]
public class ItemUpgradeData : ItemData
{
    [SerializeField] ItemData itemDataToUpgrade;
    [SerializeField] private int specialModifier;
        public int SpecialModifier => specialModifier;

    [SerializeField] private int valueToUnlock;

    override public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        isUnlocked = false;
        priceCurrent = priceDefault;
        upgradeCurrentValue = 0;

        if (newActionOnClick == null) actionOnClick = () => UpgradeItem();
        else actionOnClick = newActionOnClick;
    }

    public void UpgradeItem()
    {
        itemDataToUpgrade.IncreaseSpecialCurrentValue(specialModifier);
    }

    public void Unlock(int upgrValue)
    {
        if (upgrValue == valueToUnlock) isUnlocked = true;
    }


}
