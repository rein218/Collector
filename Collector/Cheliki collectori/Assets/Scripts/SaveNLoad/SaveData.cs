using UnityEngine;
using System;
[Serializable]
public class SaveData
{
    public bool isUnlocked;
    public ItemName itemName;
    public Sprite sprite;
    public int priceDefault;
    public int priceCurrent;
    public float priceIncrease;
    public int magnificationFactor;
    public int upgradeMaxValue;
    public int upgradeCurrentValue;
    public float specialDefaultValue;   
    public float specialCurrentValue;
}
