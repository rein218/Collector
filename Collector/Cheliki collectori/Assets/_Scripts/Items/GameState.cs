using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GameState
{
    public float gold;
    public List<ItemState> Items; 
    public List<ItemState> Upgrades;

    // Вспомогательные методы
    public ItemState GetItemState(string itemId) => Items.FirstOrDefault(i => i.Id == itemId);
    public ItemState GetUpgradeState(string upgradeId) => Upgrades.FirstOrDefault(u => u.Id == upgradeId);

    // Проверка требований
    public bool CheckRequirements(List<UnlockRequirement> requirements) =>
        requirements.All(r => r.IsMet(this));


    public GameState()
    {
        Items = new List<ItemState>();
        Upgrades = new List<ItemState>();
    }
}
