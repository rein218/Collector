using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GameState
{
    public long dollarsCount = 0;
    public long allTimeDollarsCount = 0;
    public long failsCount = 0;
    public List<ItemState> AllItems; 

    public ItemState GetItemState(ItemName type)
    {
        var state = AllItems.FirstOrDefault(i => i.itemType == type);
        if (state == null)
        {
            state = new ItemState { itemType = type, upgradeLevel = 0 };
            AllItems.Add(state);
        }
        return state;
    }


    // Проверка требований
    public bool CheckRequirements(List<UnlockRequirement> requirements) =>
        requirements.All(r => r.IsMet(this));


    public GameState()
    {
        AllItems = new List<ItemState>();
    }
}
