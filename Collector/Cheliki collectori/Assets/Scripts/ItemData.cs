using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    
    public SaveData GetSaveData() => new SaveData
    {
        isUnlocked = _isUnlocked,
        priceDefault = _priceDefault,
        priceCurrent = _priceCurrent,
        priceIncrease = _priceIncrease,
        magnificationFactor = _magnificationFactor,
        upgradeMaxValue = _maxLevelOfUpgrade,
        upgradeCurrentValue = _currentLevelOfUpgrade,
        specialDefaultValue = _specialDefaultValue,
        specialCurrentValue = _specialCurrentValue
    };
    
    public void LoadSaveData(SaveData data)
    {
        _isUnlocked = data.isUnlocked;
        _priceDefault = data.priceDefault;
        _priceCurrent = data.priceCurrent;
        _priceIncrease = data.priceIncrease;
        _magnificationFactor = data.magnificationFactor;
        _maxLevelOfUpgrade = data.upgradeMaxValue;
        _currentLevelOfUpgrade = data.upgradeCurrentValue;
        _specialDefaultValue = data.specialDefaultValue;
        _specialCurrentValue = data.specialCurrentValue;
    }
    
    [SerializeField] protected private bool _isUnlocked;
        public bool IsUnlocked => _isUnlocked;

    [Header("Type")]
    [FormerlySerializedAs("_itemName")] //временно не удалять, иначе юнити забудет значения
    [SerializeField] protected private ItemName _itemType;
        public ItemName ItemType => _itemType;
    
    [Header("Badger")]
    [SerializeField] protected private List<TextPoint>  _name;
        public List<TextPoint>  Name => _name;
    [SerializeField] protected private List<TextPoint>  _description;
    public List<TextPoint>  Description => _description;
    [SerializeField] protected private  Sprite _sprite;
        public Sprite Sprite => _sprite;
    [Header("Price")]
    [SerializeField] protected private  int _priceDefault;
        public int PriceDefault => _priceDefault;
    [SerializeField]  protected private  int _priceCurrent;
        public int PriceCurrent => _priceCurrent;

    [Header("Price change on every upgrade")]
    [SerializeField] protected private float _priceIncrease;
        public float PriceIncrease => _priceIncrease;
    [SerializeField] protected private int _magnificationFactor;
        public int MagnificationFactor => _magnificationFactor;
   
    [Header("Level and max level of upgrade")]
    [FormerlySerializedAs("_upgradeMaxValue")] //временно не удалять, иначе слетят значения
    [SerializeField] protected private  int _maxLevelOfUpgrade;
        public int MaxLevelOfUpgrade => _maxLevelOfUpgrade;
    [FormerlySerializedAs("_upgradeCurrentValue")] //временно не удалять, иначе слетят значения
    [SerializeField] protected private  int _currentLevelOfUpgrade;
        public int CurrentLevelOfUpgrade => _currentLevelOfUpgrade;
    [Header("I have no idea")]
    [SerializeField] protected private float _specialDefaultValue;
        public float SpecialDefaultValue => _specialDefaultValue;
    [SerializeField] protected private float _specialCurrentValue;
        public float SpecialCurrentValue => _specialCurrentValue;

    protected ItemSorter sorter;
    protected ItemButton button;
    public UnityEvent eventOnClick {get; protected private set; }

    virtual public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        _specialCurrentValue = _specialDefaultValue;

        if (newActionOnClick != null) eventOnClick.AddListener(newActionOnClick);
        if (newUnlockOnClick != null) eventOnClick.AddListener(newUnlockOnClick);
    }


    public bool ButtonClick()
    {
        if (IsUpgradedFull() || !CurrenciesWallet.Instance.SpendDollars(PriceCurrent) || !_isUnlocked) return false;

        IncreaseUpgradeCurrentValue();
        IncreasePriceCurrentValue();

        eventOnClick?.Invoke();

        return true;
    }

    public void ApplySorter(ItemSorter itemSorter)
    {
        sorter = itemSorter;
    }

    public void ApplyButton(ItemButton itembutton)
    {
        button = itembutton;
    }

    public void IncreaseUpgradeCurrentValue()
    {
        _currentLevelOfUpgrade++;
    }
    public void IncreasePriceCurrentValue()
    {
        if (_magnificationFactor>0)
        _priceIncrease = _priceIncrease*_magnificationFactor/100;
        _priceCurrent = (int)(_priceCurrent + _priceIncrease);
    }


    public void IncreaseSpecialCurrentValueAdd(float specialModifierValue)
    {
        _specialCurrentValue += specialModifierValue;
    }

    public void IncreaseSpecialCurrentValueMultiply(float specialModifierValue)
    {
        _specialCurrentValue *= specialModifierValue;
    }

    public bool IsUpgradedFull()
    {
        return _currentLevelOfUpgrade >= _maxLevelOfUpgrade;
    }

    private void OnDisable()
    {
        eventOnClick?.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        eventOnClick?.RemoveAllListeners();
    }
}
