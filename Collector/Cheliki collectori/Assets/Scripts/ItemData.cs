using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public SaveData GetSaveData() => new SaveData
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

    protected bool _isUnlocked;
    [SerializeField] protected private ItemName _itemName;
        public ItemName ItemName => _itemName;
    [SerializeField] protected private  Sprite _sprite;
        public Sprite Sprite => _sprite;
    [SerializeField] protected private  int _priceDefault;
        public int PriceDefault => _priceDefault;
    protected private  int _priceCurrent;
        public int PriceCurrent => _priceCurrent;
    [SerializeField] protected private  int _priceModifierOnUpgrade;
        public int PriceModifierOnUpgrade => _priceModifierOnUpgrade;
    [SerializeField] protected private  int _upgradeMaxValue;
        public int UpgradeMaxValue => _upgradeMaxValue;
    protected private  int _upgradeCurrentValue;
        public int UpgradeCurrentValue => _upgradeCurrentValue;
    [SerializeField] private float _specialDefaultValue;
        public float SpecialDefaultValue => _specialDefaultValue;
    private  float _specialCurrentValue;
        public float SpecialCurrentValue => _specialCurrentValue;


    public UnityEvent eventOnClick {get; protected private set; }

    virtual public void Init (UnityAction newActionOnClick, UnityAction newUnlockOnClick)
    {
        _isUnlocked = true;
        _priceCurrent = _priceDefault;
        _upgradeCurrentValue = 0;
        _specialCurrentValue = _specialDefaultValue;

        Debug.Log($"specialCurrentValue == {_specialCurrentValue}");

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

    public void IncreaseUpgradeCurrentValue()
    {
        _upgradeCurrentValue++;
    }
    public void IncreasePriceCurrentValue()
    {
        _priceCurrent += _priceModifierOnUpgrade;
    }
    public void IncreaseSpecialCurrentValueAdd(float specialModifierValue)
    {
        _specialCurrentValue += specialModifierValue;

        Debug.Log($"specialCurrentValue=={specialCurrentValue}");
    }

    public void IncreaseSpecialCurrentValueMultiply(float specialModifierValue)
    {
        specialCurrentValue *= specialModifierValue;

        Debug.Log($"specialCurrentValue=={specialCurrentValue}");
    }

    public bool IsUpgradedFull()
    {
        return _upgradeCurrentValue >= _upgradeMaxValue;
    }

    private void OnDestroy()
    {
        eventOnClick.RemoveAllListeners();
    }
    public void CopyTo(ItemData toWhom)
    {
        toWhom._isUnlocked = this._isUnlocked;
        toWhom._itemName = this._itemName;
        toWhom._sprite = this._sprite;
        toWhom._priceDefault = this._priceDefault;
        toWhom._priceCurrent = this._priceCurrent;
        toWhom._priceModifierOnUpgrade = this._priceModifierOnUpgrade;
        toWhom._upgradeMaxValue = this._upgradeMaxValue;
        toWhom._upgradeCurrentValue = this._upgradeCurrentValue;
        toWhom._specialDefaultValue = this._specialDefaultValue;
        toWhom._specialCurrentValue = this._specialCurrentValue;
    }

    public void CopyFrom(ItemData fromWhom)
    {
        this._isUnlocked = fromWhom._isUnlocked;
        this._itemName = fromWhom._itemName;
        this._sprite = fromWhom._sprite;
        this._priceDefault = fromWhom._priceDefault;
        this._priceCurrent = fromWhom._priceCurrent;
        this._priceModifierOnUpgrade = fromWhom._priceModifierOnUpgrade;
        this._upgradeMaxValue = fromWhom._upgradeMaxValue;
        this._upgradeCurrentValue = fromWhom._upgradeCurrentValue;
        this._specialDefaultValue = fromWhom._specialDefaultValue;
        this._specialCurrentValue = fromWhom._specialCurrentValue;
    }
}
