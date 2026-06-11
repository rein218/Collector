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

    public ItemState GetItemState(string id)
    {
        var state = AllItems.FirstOrDefault(i => i.id == id);
        if (state == null)
        {
            state = new ItemState {id = id, upgradeLevel = 0};
            AllItems.Add(state);
        }
        return state;
    }
    public int GetItemLevel(string id)
    {
        var state = AllItems.FirstOrDefault(i => i.id == id);
        if (state == null)
        {
            state = new ItemState {id = id, upgradeLevel = 0};
            AllItems.Add(state);
        }
        return state.upgradeLevel;
    }

    public GameState()
    {
        AllItems = new List<ItemState>();
    }
}
