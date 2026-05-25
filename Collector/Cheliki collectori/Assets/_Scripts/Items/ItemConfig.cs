using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemConfig : ScriptableObject
{
    public string  id;
    public ItemName itemType;
    public string DisplayName;
    public float BaseCost;
    public float CostMultiplier = 1.15f;
    public List<UpgradeConfig> Upgrades;
    public List<UnlockRequirement> UnlockRequirements;
}
