using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemUpgradeData")]
[Preserve]
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
    [Header("I have no idea")]
    [SerializeField] private ItemData itemDataToUpgrade;
    public ItemData ItemDataToUpgrade => itemDataToUpgrade;
    [SerializeField] private float _specialModifier;
        public float SpecialModifier => _specialModifier;
    [SerializeField] private int _valueToUnlock;

    override public void Init(UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        //_isUnlocked = false;

        eventOnClick.AddListener(() => UpgradeItem());
        if (newActionOnClick != null) eventOnClick.AddListener(newActionOnClick);

        if (_valueToUnlock == 0) TryToUnlock(0);
    }

    override public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick, UnityAction newUnlockOnClick2)
    {
        //_isUnlocked = false;
        eventOnClick.AddListener(() => UpgradeItem());
        if (newActionOnClick != null) eventOnClick.AddListener(newActionOnClick);

        if (_valueToUnlock == 0) TryToUnlock(0);
    }

    override public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick, UnityAction newUnlockOnClick2, UnityAction newUnlockOnClick3)
    {
        //_isUnlocked = false;
        eventOnClick.AddListener(() => UpgradeItem());
        if (newActionOnClick != null) eventOnClick.AddListener(newActionOnClick);

        if (_valueToUnlock == 0) TryToUnlock(0);
    }

    public void UpgradeItem()
    {
         itemDataToUpgrade.IncreaseSpecialCurrentValueAdd(_specialModifier);
    }

    public bool TryToUnlock(int upgrValue)
    {
        Debug.Log("_valueToUnlock "+_valueToUnlock + " " + +upgrValue);
        if (upgrValue >= _valueToUnlock)
        {
            _isUnlocked = true;
            
            button?.SetNewValues();
            sorter?.UpdateItemStatus(this, true);
        }
        
        return _isUnlocked;
    }


}
