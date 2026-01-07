using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemUpgradeData")]
public class ItemUpgradeData : ItemData
{
    [SerializeField] private ItemData itemDataToUpgrade;
    public ItemData ItemDataToUpgrade => itemDataToUpgrade;
    [SerializeField] private float specialModifier;
        public float SpecialModifier => specialModifier;

    [SerializeField] private int valueToUnlock;

    override public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        isUnlocked = false;
        priceCurrent = priceDefault;
        upgradeCurrentValue = 0;

        eventOnClick.AddListener(() => UpgradeItem());
        eventOnClick.AddListener(newActionOnClick);
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
