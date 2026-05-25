using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Item/UpgradeConfig")]
public class UpgradeConfig : ScriptableObject
{
    public string Id;            
    public string DisplayName;
    public string Description;
    public ItemConfig TargetItem;     
    public float BaseCost;
    public float CostMultiplier = 1.15f;

    public List<UnlockRequirement> UnlockRequirements;
}
