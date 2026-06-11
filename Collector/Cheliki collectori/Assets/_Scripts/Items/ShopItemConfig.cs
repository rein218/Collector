using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemConfig", menuName = "Item/ShopItemConfig")]
public class ShopItemConfig : ScriptableObject
{
    [SerializeField, ReadOnly(true)] private string id; 
    public string Id => id;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString().Substring(0, 8); 
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    //public ItemName itemType;
    public Sprite Icon;
    public ShopTab shopTab = ShopTab.Upgrade;
    public string nameKey = "testname";
    public string descKey = "testdesc";
    public int MaxLevel;
    public float BaseCost;
    public float CostAdd;
    public float CostAddMultiplier = 1.05f;
    public List<ShopItemConfig> Upgrades;
    public List<UnlockRequirement> UnlockRequirements;

    //if coin
    [Header("Coin")]
    public float BaseIncome;
    public float BaseTossDuration;
    [Header("Coin upgrade")]
    public float Add; 
    public float MultAdd; 
    public float MoveDurMinus;
    public float DoubleFlipChance;
    //if chel
    [Header("Helper")]
    public float BaseMoveSpeed;
    [Header("Helper upgrade")]
    public float MoveAdd;
}
public enum ItemType
{
    NewCoinBronze, NewCoinSilver, NewCoinGold, NewHelper, //покупка новых монет и помощников

    UpgradeCoinBronzeValueFlat, UpgradeCoinSilverValueFlat, UpgradeCoinGoldValueFlat, //увеличивает income монеты
    UpgradeCoinBronzeValuePercent, UpgradeCoinSilverValuePercent, UpgradeCoinGoldValuePercent,  //увеличивает income монеты
    UpgradeCoinMoveDurationBronze, UpgradeCoinMoveDurationSilver, UpgradeCoinMoveDurationGold, //монета подбрасывается быстрее



    DoubleFlipChanceForBronse, DoubleFlipChanceForSilver, DoubleFlipChanceForGold,  //шанс двойного подброса монеты
    FeatureMouseHoverForBronse, FeatureMouseHoverForSilver, FeatureMouseHoverForGold, // позволят подкидывать монеты просто водя мышкой по экрану (без клика)



    FeatureUnlockCoinSilverForHelper, FeatureUnlockCoinGoldForHelper, //разблокирует возможность подбрасывать монету помощнику
    UpgradeHelperSpeed,  //увеличивает скорость помощника

}

public enum ShopTab
{
    Item,
    Upgrade,
    Feature,
}
