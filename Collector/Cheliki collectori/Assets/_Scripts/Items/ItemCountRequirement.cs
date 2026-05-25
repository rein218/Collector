using UnityEngine;

[CreateAssetMenu(menuName = "Item/Requirements/ItemCount")]
public class ItemCountRequirement : UnlockRequirement
{
    public ItemConfig Item;
    public int RequiredCount = 5;

    public override bool IsMet(GameState state)
    {
        var itemState = state.GetItemState(Item.id);
        return itemState != null && itemState.UpgradeLevel >= RequiredCount;
    }
}
